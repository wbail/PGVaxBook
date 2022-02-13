using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGVaxBook.ApplicationService.Agendamento;
using PGVaxBook.ApplicationService.ConsultaAgendamento;
using PGVaxBook.ApplicationService.Validations;
using PGVaxBook.Infra.Files;
using PGVaxBook.Services.Agendamento;
using PGVaxBook.Services.ConsultaAgendamento;

namespace PGVaxBook.Presentation.Console.Configuration;

public static class DependencyInvertionConfiguration
{
    public static async Task<IHost> Configure()
    {
        using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services
                        .AddScoped<IAgendamentoApplicationService, AgendamentoApplicationService>()
                        .AddScoped<IConsultaAgendamentoApplicationService, ConsultaAgendamentoApplicationService>()
                        .AddScoped<IAgendamentoService, AgendamentoService>()
                        .AddScoped<IConsultaAgendamentoService, ConsultaAgendamentoService>()
                        .AddScoped<IArgsValidationService, ArgsValidationService>()
                        .AddSingleton<RecordsConfiguration>())
                .Build();

        return host;
    }

    public static ServiceProvider BuildServicesProvider()
    {
        IServiceCollection services = new ServiceCollection();
        services
            .AddScoped<IAgendamentoApplicationService, AgendamentoApplicationService>()
            .AddScoped<IConsultaAgendamentoApplicationService, ConsultaAgendamentoApplicationService>()
            .AddScoped<IAgendamentoService, AgendamentoService>()
            .AddScoped<IConsultaAgendamentoService, ConsultaAgendamentoService>()
            .AddScoped<IArgsValidationService, ArgsValidationService>()
            .AddSingleton<RecordsConfiguration>();
        
        return services.BuildServiceProvider();
    }

    public static IHost BuildHost()
    {
        using IHost host = Host.CreateDefaultBuilder()
                .Build();

        return host;
    }
}
