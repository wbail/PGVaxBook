namespace PGVaxBook.Messages.Requests;

public class AgendamentoApplicationRequest
{
    public AgendamentoApplicationRequest()
    {

    }

    public AgendamentoApplicationRequest(string idPrograma,
                                         string nome,
                                         string dataNascimento,
                                         string sexo,
                                         string cpf,
                                         string cartao_Sus,
                                         string celular,
                                         string nome_Mae,
                                         string cep,
                                         string regiao,
                                         string logradouro,
                                         string numero,
                                         string complemento)
    {
        IdPrograma = idPrograma;
        Nome = nome;
        DataNascimento = dataNascimento;
        Sexo = sexo;
        Cpf = cpf;
        Cartao_Sus = cartao_Sus;
        Celular = celular;
        Nome_Mae = nome_Mae;
        Cep = cep;
        Regiao = regiao;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
    }

    public string IdPrograma { get; set; }
    public string Nome { get; set; }
    public string DataNascimento { get; set; }
    public string Sexo { get; set; }
    public string Cpf { get; set; }
    public string Cartao_Sus { get; set; }
    public string Celular { get; set; }
    public string Nome_Mae { get; set; }
    public string Cep { get; set; }
    public string Regiao { get; set; }
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string Complemento { get; set; }
}
