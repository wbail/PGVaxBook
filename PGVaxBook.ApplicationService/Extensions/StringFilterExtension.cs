namespace PGVaxBook.ApplicationService.Extensions
{
    public static class StringFilterExtension
    {
        public static string Filter(this string str, List<string> stringsToRemove)
        {
            foreach (var c in stringsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }
    }
}
