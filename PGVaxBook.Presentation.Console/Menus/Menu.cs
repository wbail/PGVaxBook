using PGVaxBook.Presentation.Console.Menus.Enums;

namespace PGVaxBook.Presentation.Console.Menus;

public static class Menu
{
    public static MenuOptionsEnum Option(char op) => op switch
    {
        '1' => MenuOptionsEnum.Consulta,
        '2' => MenuOptionsEnum.Agendamento,
        _ => throw new ArgumentOutOfRangeException(nameof(op), $"A opcao {op} nao existe."),
    };

    public static void ListOptions()
    {
        System.Console.WriteLine("Escolha uma opcao:");
        System.Console.WriteLine("1 - Consulta");
        System.Console.WriteLine("2 - Agendamento");
    }

    public static char ReadOption()
    {
        System.Console.Write("Option: ");
        var op = System.Console.ReadKey().KeyChar;
        return op;
    }
}

