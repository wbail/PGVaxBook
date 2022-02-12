using Newtonsoft.Json;
using PGVaxBook.Messages.Requests;
using PGVaxBook.Messages.Responses;
using System.Text.Json;

namespace PGVaxBook.Services.Agendamento;

public class AgendamentoService : IAgendamentoService
{
    private readonly HttpClient _httpClient;
    private readonly string _uri = "https://fms.pontagrossa.pr.gov.br/vacinacao";

    public AgendamentoService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<AgendamentoApplicationResponse> MakeAgendamento(AgendamentoApplicationRequest agendamentoRequest)
    {
        return await PostAsync(agendamentoRequest);
    }

    private async Task<AgendamentoApplicationResponse> PostAsync(AgendamentoApplicationRequest agendamentoRequest)
    {
        var url = $"{_uri}/app/store";
        Uri uri = new Uri(url);

        var formContent = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("id_programa", agendamentoRequest.IdPrograma),
                new KeyValuePair<string, string>("nome", agendamentoRequest.Nome),
                new KeyValuePair<string, string>("data_nascimento", agendamentoRequest.DataNascimento),
                new KeyValuePair<string, string>("sexo", agendamentoRequest.Sexo),
                new KeyValuePair<string, string>("cpf", agendamentoRequest.Cpf),
                new KeyValuePair<string, string>("cartao_sus", agendamentoRequest.Cartao_Sus),
                new KeyValuePair<string, string>("celular", agendamentoRequest.Celular),
                new KeyValuePair<string, string>("nome_mae", agendamentoRequest.Nome_Mae),
                new KeyValuePair<string, string>("cep", agendamentoRequest.Cep),
                new KeyValuePair<string, string>("regiao", agendamentoRequest.Regiao),
                new KeyValuePair<string, string>("logradouro", agendamentoRequest.Logradouro),
                new KeyValuePair<string, string>("numero", agendamentoRequest.Numero),
                new KeyValuePair<string, string>("complemento", agendamentoRequest.Complemento),
            });

        var response = await _httpClient.PostAsync(uri, formContent);

        var responseMessage = await response.Content.ReadAsStringAsync();

        //var agendamentoResponse = await JsonSerializer.DeserializeAsync<AgendamentoApplicationResponse>(responseMessage);
        var agendamentoResponse = JsonConvert.DeserializeObject<AgendamentoApplicationResponse>(responseMessage);

        return agendamentoResponse;
    }
}
