using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PGVaxBook.ApplicationService.Agendamento;
using PGVaxBook.ApplicationService.ConsultaAgendamento;
using PGVaxBook.Infra.Files;
using PGVaxBook.Services.Agendamento;
using PGVaxBook.Services.ConsultaAgendamento;

namespace PGVaxBook.DI;

public static class DIFactory
{
    public static void ConfigureDI(this IServiceCollection services, IConfiguration configuration)
    {
        #region Application Layer

        services.AddScoped<IAgendamentoApplicationService, AgendamentoApplicationService>();
        services.AddScoped<IConsultaAgendamentoApplicationService, ConsultaAgendamentoApplicationService>();

        #endregion

        #region Service Layer

        services.AddScoped<IAgendamentoService, AgendamentoService>();
        services.AddScoped<IConsultaAgendamentoService, ConsultaAgendamentoService>();

        #endregion

        #region Infrastructure Layer

        var lines = File.ReadAllLines(@"c:\records.txt");
        var recordsConfiguration = new RecordsConfiguration();
        recordsConfiguration.Lines = lines.ToList();
        services.AddSingleton(recordsConfiguration);

        #endregion
    }

    #region Private Methods

    private static IMapper RegisterAutoMapper()
    {
        var mappingConfig = new MapperConfiguration(configuration =>
        {
            //configuration.AddProfile<HotelReservationMapper>();
        });

        return mappingConfig.CreateMapper();
    }

    #endregion
}
