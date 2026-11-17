namespace UserManagementApi.Enums;

public enum PasswordValidationError
{
    PasswordEmpty,
    PasswordTooShort,
    NoLetter,
    NoDigit,
    NoSpecialCharacter
}