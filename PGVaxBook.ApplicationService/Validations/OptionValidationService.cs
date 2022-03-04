namespace PGVaxBook.ApplicationService.Validations;

public class OptionValidationService : IOptionValidationService
{
    public OptionValidationService()
    {

    }

    public bool IsInvalidOption(int option)
    {
        //return option < 1 || option > (_linesConsulta.Keys.Count + 1) ? true : false;
        return (int)(option - '0') < 1 ? true : false;
    }
}
