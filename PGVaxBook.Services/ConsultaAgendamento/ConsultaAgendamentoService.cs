using Newtonsoft.Json;
using PGVaxBook.Messages.Requests;
using PGVaxBook.Messages.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        /*
         <h2 class="text-center"><i>COMPROVANTE DE AGENDAMENTO</i><br /><u>COVID-19</u></h2><br/><br/>
<div class="alert alert-danger" role="alert">
O CPF 03731137909 consultado está pendente de distribuição de horário. <br/>Aguarde até as vagas esgotarem ou após as 19h para consultar novamente.
        </div><br/><i>Comprovante gerado em 09/02/2022 19:06:55</i>

         */

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

        //var consultaAgendamentoResponse = await JsonSerializer.DeserializeAsync<ConsultaAgendamentoApplicationResponse>(responseMessage);
        //var consultaAgendamentoResponse = JsonConvert.DeserializeObject<ConsultaAgendamentoApplicationResponse>(responseMessage);
        var consultaAgendamentoResponse = new ConsultaAgendamentoApplicationResponse();
        consultaAgendamentoResponse.Html = responseMessage;
        return consultaAgendamentoResponse;
    }
}
