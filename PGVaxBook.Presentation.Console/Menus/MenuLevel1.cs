using PGVaxBook.Messages.Requests;

namespace PGVaxBook.Presentation.Console.Menus;

public static class MenuLevel1
{
    private static Dictionary<int, AgendamentoApplicationRequest> _linesAgendamento = new Dictionary<int, AgendamentoApplicationRequest>();
    private static Dictionary<int, ConsultaAgendamentoApplicationRequest> _linesConsulta = new Dictionary<int, ConsultaAgendamentoApplicationRequest>();

    public static void Build(List<string> records)
    {
        var i = 1;
        foreach (string line in records.Skip(1))
        {
            var field = line.Split(',');

            var agendamentoRequest = new AgendamentoApplicationRequest();
            agendamentoRequest.Cartao_Sus = field[0];
            agendamentoRequest.Celular = field[1];
            agendamentoRequest.Cep = field[2];
            agendamentoRequest.Complemento = field[3];
            agendamentoRequest.DataNascimento = field[4];
            agendamentoRequest.IdPrograma = field[5];
            agendamentoRequest.Logradouro = field[6];
            agendamentoRequest.Nome = field[7];
            agendamentoRequest.Nome_Mae = field[8];
            agendamentoRequest.Numero = field[9];
            agendamentoRequest.Regiao = field[10];
            agendamentoRequest.Sexo = field[11];
            agendamentoRequest.Cpf = field[12];

            _linesAgendamento.Add(i, agendamentoRequest);

            var consultaAgendamentoApplicationRequest = new ConsultaAgendamentoApplicationRequest(agendamentoRequest.Cpf);
            _linesConsulta.Add(i, consultaAgendamentoApplicationRequest);

            i++;
        }
    }

    public static List<AgendamentoApplicationRequest> OptionAgendamento(char op)
    {
        var option = (int)(op - '0');

        var list = new List<AgendamentoApplicationRequest>();

        if (option == (_linesAgendamento.Keys.Count + 1))
        {
            return _linesAgendamento.Values.ToList();
        }

        var agendamentoApplicationRequest = _linesAgendamento.Where(x => x.Key == option).FirstOrDefault();

        list.Add(agendamentoApplicationRequest.Value);

        return list;
    }

    public static List<ConsultaAgendamentoApplicationRequest> OptionConsulta(char op)
    {
        var option = (int)(op - '0');

        var list = new List<ConsultaAgendamentoApplicationRequest>();

        if (option == (_linesConsulta.Keys.Count + 1))
        {
            return _linesConsulta.Values.ToList();
        }

        var consultaAgendamentoApplicationRequest = _linesConsulta.Where(x => x.Key == option).FirstOrDefault();
        list.Add(consultaAgendamentoApplicationRequest.Value);
        return list;
    }

    public static void ListOptions()
    {
        System.Console.WriteLine("\n\nEscolha uma pessoa:");

        foreach (var line in _linesAgendamento)
        {
            System.Console.WriteLine($"{line.Key} - {line.Value.Nome}");
        }

        System.Console.WriteLine($"{_linesAgendamento.Keys.Count + 1} - Todos");
    }

    public static char ReadOption()
    {
        System.Console.Write("Option: ");
        var op = System.Console.ReadKey().KeyChar;
        return op;
    }
}
