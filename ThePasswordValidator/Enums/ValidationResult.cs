namespace ThePasswordValidator.Enums;

public enum ValidationResult
{
    Ok,
    InvalidLength,
    ForbiddenCharacters,
    NoUpperCase,
    NoLowerCase,
    NoDigit
}
