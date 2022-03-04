using Microsoft.Extensions.DependencyInjection;
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
        var argsValidationService = GetArgsValidationServiceInstance(serviceProvider);
        var optionValidationService = GetOptionValidationServiceInstance(serviceProvider);

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

            var optionValidation = false;

            switch (menuOptionEnum)
            {
                case MenuOptionsEnum.Consulta:
                    MenuLevel1.ListOptions();
                    option = MenuLevel1.ReadOption();

                    optionValidation = optionValidationService.IsInvalidOption(option);

                    if (optionValidation)
                    {
                        System.Console.WriteLine("Invalid option.");
                        return;
                    }

                    var consultaRequest = MenuLevel1.OptionConsulta(option);
                    System.Console.WriteLine("\nExecuting Consulta");

                    var result = await consultaAgendamentoApplicationService.ConsultaAgendamento(consultaRequest, 7000);
                    result.ForEach(x => System.Console.WriteLine(x));

                    break;
                case MenuOptionsEnum.Agendamento:
                    MenuLevel1.ListOptions();
                    option = MenuLevel1.ReadOption();

                    optionValidation = optionValidationService.IsInvalidOption(option);

                    if (optionValidation)
                    {
                        System.Console.WriteLine("Invalid option.");
                        return;
                    }

                    var agendamentoRequest = MenuLevel1.OptionAgendamento(option);
                    System.Console.WriteLine("\nExecuting Agendamento");

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

    private static IArgsValidationService GetArgsValidationServiceInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var argsValidationService = scope.ServiceProvider.GetService<IArgsValidationService>();
        return argsValidationService;
    }

    private static IOptionValidationService GetOptionValidationServiceInstance(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var optionValidationService = scope.ServiceProvider.GetService<IOptionValidationService>();
        return optionValidationService;
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
