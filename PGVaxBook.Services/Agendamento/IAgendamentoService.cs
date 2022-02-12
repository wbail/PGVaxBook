using PGVaxBook.Messages.Requests;
using PGVaxBook.Messages.Responses;

namespace PGVaxBook.Services.Agendamento;

public interface IAgendamentoService
{
    Task<AgendamentoApplicationResponse> MakeAgendamento(AgendamentoApplicationRequest agendamentoRequest);
}
