using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGVaxBook.ApplicationService.Agendamento;
using PGVaxBook.ApplicationService.ConsultaAgendamento;
using PGVaxBook.ApplicationService.Validations;
using PGVaxBook.Infra.Files;
using PGVaxBook.Presentation.Console.Menus;
using PGVaxBook.Presentation.Console.Menus.Enums;

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

            MenuLevel1.Build(recordsConfiguration.Lines);

            Menu.ListOptions();

            var option = Menu.ReadOption();

            var menuOptionEnum = Menu.Option(option);

            switch (menuOptionEnum)
            {
                case MenuOptionsEnum.Consulta:
                    MenuLevel1.ListOptions();
                    option = MenuLevel1.ReadOption();
                    var consultaRequest = MenuLevel1.OptionConsulta(option);
                    System.Console.WriteLine("\nExecuting Consulta");

                    var result = new List<string>();
                    result = await consultaAgendamentoApplicationService.ConsultaAgendamento(consultaRequest, 7000);
                    result.ForEach(x => System.Console.WriteLine(x));

                    break;
                case MenuOptionsEnum.Agendamento:
                    MenuLevel1.ListOptions();
                    option = MenuLevel1.ReadOption();
                    var agendamentoRequest = MenuLevel1.OptionAgendamento(option);
                    System.Console.WriteLine("\nExecuting Agendamento");

                    result = new List<string>();
                    result = await agendamentoApplicationService.MakeAgendamento(agendamentoRequest, 7000);
                    result.ForEach(x => System.Console.WriteLine(x));

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
}
