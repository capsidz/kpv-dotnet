using System.Text.RegularExpressions;
using UserManagementApi.Enums;

namespace UserManagementApi.Validation;

public class RegistrationValidator
{
    public List<ValidationError> Validate(
        string name,
        string login,
        string email,
        string password)
    {
        var errors = new List<ValidationError>();

        errors.AddRange(ValidateName(name));
        errors.AddRange(ValidateLogin(login));
        errors.AddRange(ValidateEmail(email));
        errors.AddRange(ValidatePassword(password));

        return errors;
    }

    private List<ValidationError> ValidateName(string name)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(ValidationError.NameEmpty);
        }

        return errors;
    }

    private List<ValidationError> ValidateLogin(string login)
    {
        var errors = new List<ValidationError>();
        
        if (string.IsNullOrWhiteSpace(login))
        {
            errors.Add(ValidationError.LoginEmpty);
            return errors;
        }
        
        if (login.Length < 3) errors.Add(ValidationError.LoginTooShort);
        
        if (login.Length > 16) errors.Add(ValidationError.LoginTooLong);
        
        return errors;
    }
    
    private List<ValidationError> ValidateEmail(string email)
    {
        var errors = new List<ValidationError>();
        
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(ValidationError.EmailEmpty);
            return errors;
        }
        
        if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase)) errors.Add(ValidationError.InvalidEmail);
        
        return errors;
    }
    
    private List<ValidationError> ValidatePassword(string password)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add(ValidationError.PasswordEmpty);
            return errors;
        }
        
        if (password.Length < 6) errors.Add(ValidationError.PasswordTooShort);
        
        if (!Regex.IsMatch(password, @"[a-zA-Z]")) errors.Add(ValidationError.NoLetter);

        if (!Regex.IsMatch(password, @"[0-9]")) errors.Add(ValidationError.NoDigit);
        
        if (!Regex.IsMatch(password, @"[\p{P}\p{S}]")) errors.Add(ValidationError.NoSpecialCharacter);

        return errors;
    }
}