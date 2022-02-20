using PGVaxBook.Messages.Requests;

namespace PGVaxBook.ApplicationService.Agendamento;

public interface IAgendamentoApplicationService
{
    Task<List<string>> MakeAgendamento(List<AgendamentoApplicationRequest> agendamentoRequests, int delayInMilliseconds);
}
