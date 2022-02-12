using PGVaxBook.Messages.Requests;
using PGVaxBook.Services.Agendamento;
using System.Text.RegularExpressions;

namespace PGVaxBook.ApplicationService.Agendamento;

public class AgendamentoApplicationService : IAgendamentoApplicationService
{
    private readonly IAgendamentoService _agendamentoService;

    public AgendamentoApplicationService(IAgendamentoService agendamentoService)
    {
        _agendamentoService = agendamentoService;
    }

    public async Task<string> MakeAgendamento(AgendamentoApplicationRequest agendamentoRequest)
    {
        var agendamentoResponse = await _agendamentoService.MakeAgendamento(agendamentoRequest);

        if (agendamentoResponse.Erro)
        {
            var pattern = @">\W+[a-zA-Z]\W.+";
            var errorMessage = Regex.Match(agendamentoResponse.Html, pattern).Value;
            return errorMessage;
        }

        return $"Agendamento efetuado para {agendamentoRequest.Nome}";
    }
}
