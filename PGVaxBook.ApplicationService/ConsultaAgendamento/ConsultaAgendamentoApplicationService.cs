using PGVaxBook.Messages.Requests;
using PGVaxBook.Services.ConsultaAgendamento;

namespace PGVaxBook.ApplicationService.ConsultaAgendamento;

public class ConsultaAgendamentoApplicationService : IConsultaAgendamentoApplicationService
{
    private readonly IConsultaAgendamentoService _consultaAgendamentoService;

    public ConsultaAgendamentoApplicationService(IConsultaAgendamentoService consultaAgendamentoService)
    {
        _consultaAgendamentoService = consultaAgendamentoService;
    }

    public async Task<bool> ConsultaAgendamento(ConsultaAgendamentoApplicationRequest consultaAgendamentoRequest)
    {
        var response = await _consultaAgendamentoService.ConsultaAgendamento(consultaAgendamentoRequest);

        return IsConsultaAgendamentoExists(response.Html);
    }

    private bool IsConsultaAgendamentoExists(string responseMessage)
    {
        //var exists = responseMessage.Contains("consultado está pendente") || responseMessage.Contains("não foi agendado");

        var exists = responseMessage.Contains("Agendado para ");

        return exists;
    }
}
