namespace UserManagementApi.Enums;

public enum ValidationError
{
    NameEmpty,
    
    LoginEmpty,
    LoginTooShort,
    LoginTooLong,
    LoginAlreadyExists,
    
    EmailEmpty,
    InvalidEmail,
    
    PasswordEmpty,
    PasswordTooShort,
    NoLetter,
    NoDigit,
    NoSpecialCharacter
}