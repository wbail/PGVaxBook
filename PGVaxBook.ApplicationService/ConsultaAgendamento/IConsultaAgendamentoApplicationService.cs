using PGVaxBook.Messages.Requests;

namespace PGVaxBook.ApplicationService.ConsultaAgendamento;

public interface IConsultaAgendamentoApplicationService
{
    Task<bool> ConsultaAgendamento(ConsultaAgendamentoApplicationRequest consultaAgendamentoRequest);
}
