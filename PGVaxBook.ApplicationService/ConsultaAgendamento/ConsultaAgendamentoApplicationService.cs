using PGVaxBook.ApplicationService.Extensions;
using PGVaxBook.Messages.Requests;
using PGVaxBook.Services.ConsultaAgendamento;
using System.Text.RegularExpressions;

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
                var messageParsed = ParseResponseMessage(response.Html);
                consultaAgendamentoResponseList.Add($"{messageParsed} para o CPF {consultaAgendamentoRequest.Cpf}");
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

    private string ParseResponseMessage(string message)
    {
        var pattern = @"(?<=<p>)(.*?)(?=<\/p>)";
        var stringsToRemove = new List<string>() { "<strong>", "</strong>", "<i>", "</i>", "<br>", "</br>", "<br/>" };

        var occurrence = Regex.Match(message, pattern).Value;

        var newMessage = occurrence.Filter(stringsToRemove);

        return newMessage;
    }
}
