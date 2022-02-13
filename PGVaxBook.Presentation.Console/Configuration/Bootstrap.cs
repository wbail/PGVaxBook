using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGVaxBook.ApplicationService.Agendamento;
using PGVaxBook.ApplicationService.ConsultaAgendamento;
using PGVaxBook.ApplicationService.Validations;
using PGVaxBook.Infra.Files;
using PGVaxBook.Messages.Requests;

namespace PGVaxBook.Presentation.Console.Configuration;

public static class Bootstrap
{
    public static async Task RunAsync(string[] args)
    {
        var host = DependencyInvertionConfiguration.BuildHost();
        var serviceProvider = DependencyInvertionConfiguration.BuildServicesProvider();

        var consultaAgendamentoApplicationService = GetConsultaAgendamentoApplicationServiceInstance(serviceProvider);
        var agendamentoApplicationService = GetAgendamentoApplicationServiceInstance(serviceProvider);
        var recordsConfiguration = GetRecordsConfigurationInstance(serviceProvider);
        var argsValidationService = GetValidationServiceInstance(serviceProvider);

        var isSingleArgument = argsValidationService.IsSingleArgument(args);

        if (!isSingleArgument)
        {
            System.Console.Error.WriteLine("Accepts only one argument. Eg: C:\\Users\\JoeDoe\\Desktop\\records.txt");
            Environment.Exit(0);
        }

        var path = argsValidationService.IsFilePathExists(args);

        if (path)
        {
            recordsConfiguration.Lines = ReadFileConfiguration.ReadFile(args);

            var agendamentosRequest = GetAgendamentoApplicationRequests(recordsConfiguration.Lines);

            var cpfs = GetCpfsFromAgendamentoApplicationRequests(agendamentosRequest);

            var consultasAgendamentoRequest = GetConsultaAgendamentoApplicationRequests(cpfs);


            Menu.ListOptions();

            var option = Menu.ReadOption();

            var menuOptionEnum = Menu.Option(option);

            switch (menuOptionEnum)
            {
                case MenuOptionsEnum.Consulta:
                    System.Console.WriteLine("\nExecuting Consulta");
                    var result = await consultaAgendamentoApplicationService.ConsultaAgendamento(consultasAgendamentoRequest, 7000);
                    System.Console.WriteLine($"{result.FirstOrDefault()}");
                    break;
                case MenuOptionsEnum.Agendamento:
                    System.Console.WriteLine("\nExecuting Agendamento");
                    await agendamentoApplicationService.MakeAgendamento(agendamentosRequest, 7000);
                    break;
                default:
                    break;
            }

            //host.Run();
        }
        else
        {
            System.Console.Error.WriteLine("Needs to pass the file path.");
            Environment.Exit(0);
        }
    }

    private static IArgsValidationService GetValidationServiceInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var argsValidationService = scope.ServiceProvider.GetService<IArgsValidationService>();
        return argsValidationService;
    }

    private static IAgendamentoApplicationService GetAgendamentoApplicationServiceInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var agendamentoApplicationService = scope.ServiceProvider.GetService<IAgendamentoApplicationService>();
        return agendamentoApplicationService;
    }

    private static IConsultaAgendamentoApplicationService GetConsultaAgendamentoApplicationServiceInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var consultaAgendamentoApplicationService = scope.ServiceProvider.GetService<IConsultaAgendamentoApplicationService>();
        return consultaAgendamentoApplicationService;
    }

    private static RecordsConfiguration GetRecordsConfigurationInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var recordsConfiguration = scope.ServiceProvider.GetService<RecordsConfiguration>();
        return recordsConfiguration;
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
}
