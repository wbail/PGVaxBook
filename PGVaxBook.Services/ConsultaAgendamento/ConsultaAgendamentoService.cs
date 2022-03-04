using PGVaxBook.Messages.Requests;
using PGVaxBook.Messages.Responses;

namespace PGVaxBook.Services.ConsultaAgendamento;

public class ConsultaAgendamentoService : IConsultaAgendamentoService
{
    private readonly HttpClient _httpClient;
    private readonly string _uri = "https://fms.pontagrossa.pr.gov.br/vacinacao";

    public ConsultaAgendamentoService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ConsultaAgendamentoApplicationResponse> ConsultaAgendamento(ConsultaAgendamentoApplicationRequest consultaAgendamentoRequest)
    {
        var url = $"{_uri}/app/consulta";
        Uri uri = new Uri(url);

        var formContent = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("cpf_consulta", consultaAgendamentoRequest.Cpf),
                new KeyValuePair<string, string>("tipo", consultaAgendamentoRequest.Tipo),
                new KeyValuePair<string, string>("cartao_sus_consulta", consultaAgendamentoRequest.CartaoSusConsulta),
            });

        var response = await _httpClient.PostAsync(uri, formContent);

        var responseMessage = await response.Content.ReadAsStringAsync();

        var consultaAgendamentoResponse = new ConsultaAgendamentoApplicationResponse();
        consultaAgendamentoResponse.Html = responseMessage;
        return consultaAgendamentoResponse;
    }
}
