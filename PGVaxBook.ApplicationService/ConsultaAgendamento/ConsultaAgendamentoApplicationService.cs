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

    public async Task<List<string>> ConsultaAgendamento(List<ConsultaAgendamentoApplicationRequest> consultaAgendamentoRequests, int delayInMilliseconds)
    {
        var consultaAgendamentoResponseList = new List<string>();

        foreach (var consultaAgendamentoRequest in consultaAgendamentoRequests)
        {
            var response = await _consultaAgendamentoService.ConsultaAgendamento(consultaAgendamentoRequest);

            var isAgendado = IsConsultaAgendamentoExists(response.Html);

            if (isAgendado)
            {
                consultaAgendamentoResponseList.Add($"O CPF {consultaAgendamentoRequest.Cpf} esta agendado");
            }
            else
            {
                consultaAgendamentoResponseList.Add($"O CPF {consultaAgendamentoRequest.Cpf} NAO esta agendado");
            }

            await Task.Delay(delayInMilliseconds);
        }

        return consultaAgendamentoResponseList;
    }

    private bool IsConsultaAgendamentoExists(string responseMessage)
    {
        //var exists = responseMessage.Contains("consultado está pendente") || responseMessage.Contains("não foi agendado");

        var exists = responseMessage.Contains("Agendado para ");

        return exists;
    }
}
