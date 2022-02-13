namespace PGVaxBook.Presentation.Console.Configuration;

public static class ReadFileConfiguration
{
    public static List<string> ReadFile(string[] args)
    {
        var lines = File.ReadAllLines(args[0]);
        return lines.ToList();
    }
}
