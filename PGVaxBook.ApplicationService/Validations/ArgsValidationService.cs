namespace PGVaxBook.ApplicationService.Validations;

public class ArgsValidationService : IArgsValidationService
{
    public bool IsFilePathExists(string[] args)
    {
        return args.Any() &&
            !string.IsNullOrEmpty(args[0]) &&
            File.Exists(args[0]);
    }

    public bool IsSingleArgument(string[] args)
    {
        return args.Any() &&
            args.Length == 1;
    }
}
