using PGVaxBook.Messages.Requests;

namespace PGVaxBook.ApplicationService.ConsultaAgendamento;

public interface IConsultaAgendamentoApplicationService
{
    Task<List<string>> ConsultaAgendamento(List<ConsultaAgendamentoApplicationRequest> consultaAgendamentoRequests, int delayInMilliseconds);
}
