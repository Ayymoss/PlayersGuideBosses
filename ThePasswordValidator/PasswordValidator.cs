using ThePasswordValidator.Enums;

namespace ThePasswordValidator;

public static class PasswordValidator
{
    private static readonly List<char> InvalidChars = ['T', '&'];

    public static IEnumerable<ValidationResult> ValidatePassword(this string password)
    {
        var failedResults = new List<ValidationResult>();
        if (password.Length is < 6 or > 13) failedResults.Add(ValidationResult.InvalidLength);
        if (password.Any(x => InvalidChars.Contains(x))) failedResults.Add(ValidationResult.ForbiddenCharacters);
        if (!password.Any(char.IsUpper)) failedResults.Add(ValidationResult.NoUpperCase);
        if (!password.Any(char.IsLower)) failedResults.Add(ValidationResult.NoLowerCase);
        if (!password.Any(char.IsDigit)) failedResults.Add(ValidationResult.NoDigit);

        if (failedResults.Count is 0) failedResults.Add(ValidationResult.Ok);

        return failedResults;
    }
}
