using PGVaxBook.Messages.Requests;
using PGVaxBook.Messages.Responses;

namespace PGVaxBook.Services.ConsultaAgendamento;

public interface IConsultaAgendamentoService
{
    Task<ConsultaAgendamentoApplicationResponse> ConsultaAgendamento(ConsultaAgendamentoApplicationRequest consultaAgendamentoRequest);
}
