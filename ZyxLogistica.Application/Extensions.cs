using System.Text.RegularExpressions;

namespace ZyxLogistica.Application;

public static class Extensions
{
    public static string CleanNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, @"\D", "");
    }
}
