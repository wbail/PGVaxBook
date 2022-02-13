using PGVaxBook.Messages.Requests;

namespace PGVaxBook.ApplicationService.Agendamento;

public interface IAgendamentoApplicationService
{
    Task<string> MakeAgendamento(AgendamentoApplicationRequest agendamentoRequest);
    Task<List<string>> MakeAgendamento(List<AgendamentoApplicationRequest> agendamentoRequests, int delayInMilliseconds);
}
