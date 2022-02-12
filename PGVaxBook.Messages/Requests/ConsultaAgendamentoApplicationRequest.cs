namespace PGVaxBook.Messages.Requests;

public class ConsultaAgendamentoApplicationRequest
{
    public ConsultaAgendamentoApplicationRequest(string cpf)
    {
        Cpf = cpf;
    }

    public string Cpf { get; set; }

    public string Tipo { get; set; }

    public string CartaoSusConsulta { get; set; }
}
