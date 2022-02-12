using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGVaxBook.ApplicationService.Agendamento;
using PGVaxBook.ApplicationService.ConsultaAgendamento;
using PGVaxBook.Infra.Files;
using PGVaxBook.Messages.Requests;
using PGVaxBook.Services.Agendamento;
using PGVaxBook.Services.ConsultaAgendamento;

namespace PGVaxBook.Presentation.Console.Configuration;

public static class Bootstrap
{
    public static async Task RunAsync(string[] args)
    {
        var isSingleArgument = IsSingleArgument(args);

        if (!isSingleArgument)
        {
            System.Console.Error.WriteLine("Accepts only one argument. Eg: C:\\Users\\JoeDoe\\Desktop\\records.txt");
            Environment.Exit(0);
        }

        var path = await IsFilePathExists(args);

        if (path)
        {
            var recordsConfiguration = new RecordsConfiguration();
            recordsConfiguration.Lines = await ReadFile(args);

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddScoped<IAgendamentoApplicationService, AgendamentoApplicationService>()
                        .AddScoped<IConsultaAgendamentoApplicationService, ConsultaAgendamentoApplicationService>()
                        .AddScoped<IAgendamentoService, AgendamentoService>()
                        .AddScoped<IConsultaAgendamentoService, ConsultaAgendamentoService>()
                        .AddSingleton<RecordsConfiguration>(recordsConfiguration))
                .Build();

            var agendamentosRequest = GetAgendamentoApplicationRequests(recordsConfiguration.Lines);

            var cpfs = GetCpfsFromAgendamentoApplicationRequests(agendamentosRequest);

            var consultasRequest = GetConsultaAgendamentoApplicationRequests(cpfs);
            
            // TODO: Uncomment to execute the methods
            //await Agendamento(host.Services, agendamentosRequest);
            //await ConsultaAgendamento(host.Services, consultasRequest);

            await host.RunAsync();
        }
        else
        {
            System.Console.Error.WriteLine("Needs to pass the file path");
            Environment.Exit(0);
        }
    }

    private static async Task<bool> IsFilePathExists(string[] args)
    {
        return args.Any() &&
            !string.IsNullOrEmpty(args[0]) &&
            File.Exists(args[0]);
    }

    private static bool IsSingleArgument(string[] args)
    {
        return args.Any() &&
            args.Length == 1;
    }

    private static async Task<List<string>> ReadFile(string[] args)
    {
        var lines = await File.ReadAllLinesAsync(args[0]);
        return lines.ToList();
    }

    private static List<string> GetCpfsFromAgendamentoApplicationRequests(List<AgendamentoApplicationRequest> agendamentoApplicationRequests)
    {
        return agendamentoApplicationRequests.Select(request => request.Cpf).ToList();
    }

    private static List<AgendamentoApplicationRequest> GetAgendamentoApplicationRequests(List<string> records)
    {
        var agendamentoRequests = new List<AgendamentoApplicationRequest>();

        foreach (string line in records.Skip(1))
        {
            var field = line.Split(',');

            var agendamentoRequest = new AgendamentoApplicationRequest();
            agendamentoRequest.Cartao_Sus = field[0];
            agendamentoRequest.Celular = field[1];
            agendamentoRequest.Cep = field[2];
            agendamentoRequest.Complemento = field[3];
            agendamentoRequest.DataNascimento = field[4];
            agendamentoRequest.IdPrograma = field[5];
            agendamentoRequest.Logradouro = field[6];
            agendamentoRequest.Nome = field[7];
            agendamentoRequest.Nome_Mae = field[8];
            agendamentoRequest.Numero = field[9];
            agendamentoRequest.Regiao = field[10];
            agendamentoRequest.Sexo = field[11];
            agendamentoRequest.Cpf = field[12];

            agendamentoRequests.Add(agendamentoRequest);
        }

        return agendamentoRequests;
    }

    private static List<ConsultaAgendamentoApplicationRequest> GetConsultaAgendamentoApplicationRequests(List<string> cpfs)
    {
        var consultaAgendamentoRequests = new List<ConsultaAgendamentoApplicationRequest>();

        foreach (var cpf in cpfs)
        {
            var consultaAgendamento = new ConsultaAgendamentoApplicationRequest(cpf);

            consultaAgendamentoRequests.Add(consultaAgendamento);
        }

        return consultaAgendamentoRequests;
    }

    private static async Task Agendamento(IServiceProvider services, List<AgendamentoApplicationRequest> agendamentoApplicationRequests)
    {
        using IServiceScope serviceScope = services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var agendamentoApplicationService = provider.GetRequiredService<IAgendamentoApplicationService>();

        foreach (var agendamentoApplicationRequest in agendamentoApplicationRequests)
        {
            var response = await agendamentoApplicationService.MakeAgendamento(agendamentoApplicationRequest);

            System.Console.WriteLine($"Result: {response}");

            await Task.Delay(7000);
        }
    }

    private static async Task ConsultaAgendamento(IServiceProvider services, List<ConsultaAgendamentoApplicationRequest> consultaAgendamentoApplicationRequests)
    {
        using IServiceScope serviceScope = services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var consultaAgendamentoApplicationService = provider.GetRequiredService<IConsultaAgendamentoApplicationService>();

        foreach (var consultaAgendamentoApplicationRequest in consultaAgendamentoApplicationRequests)
        {
            var isAgendado = await consultaAgendamentoApplicationService.ConsultaAgendamento(consultaAgendamentoApplicationRequest);

            if (isAgendado)
            {
                System.Console.WriteLine($"O CPF {consultaAgendamentoApplicationRequest.Cpf} esta agendado");
            }
            else
            {
                System.Console.WriteLine($"O CPF {consultaAgendamentoApplicationRequest.Cpf} NAO esta agendado");
            }

            await Task.Delay(7000);
        }
    }
}
