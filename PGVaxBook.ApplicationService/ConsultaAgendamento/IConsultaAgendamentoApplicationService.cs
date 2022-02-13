using PGVaxBook.Messages.Requests;

namespace PGVaxBook.ApplicationService.ConsultaAgendamento;

public interface IConsultaAgendamentoApplicationService
{
    Task<bool> ConsultaAgendamento(ConsultaAgendamentoApplicationRequest consultaAgendamentoRequest);
    Task<List<string>> ConsultaAgendamento(List<ConsultaAgendamentoApplicationRequest> consultaAgendamentoRequests, int delayInMilliseconds);
}
