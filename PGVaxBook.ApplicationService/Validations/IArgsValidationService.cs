namespace PGVaxBook.ApplicationService.Validations;

public interface IArgsValidationService
{
    bool IsFilePathExists(string[] args);
    bool IsSingleArgument(string[] args);
}
