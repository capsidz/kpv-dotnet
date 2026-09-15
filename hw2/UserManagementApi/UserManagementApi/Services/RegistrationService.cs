using Microsoft.AspNetCore.Identity;
using UserManagementApi.Entities;
using UserManagementApi.Enums;
using UserManagementApi.Repository;
using UserManagementApi.Validation;

namespace UserManagementApi.Services;

public class RegistrationService
{
    private readonly RegistrationValidator _validator;
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public RegistrationService(
        RegistrationValidator validator,
        UserRepository userRepository)
    {
        _validator = validator;
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public List<ValidationError> Register(
        string name,
        string login,
        string email,
        string password)
    {
        var errors = _validator.Validate(name, login, email, password);

        if (errors.Count > 0)
        {
            return errors;
        }
        
        if (_userRepository.GetByLogin(login) is not null)
        {
            errors.Add(ValidationError.LoginAlreadyExists);
            return errors;
        }
        
        var user = new User
        {
            Name = name,
            Login = login,
            Email = email,
            PasswordHash = ""
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        
        _userRepository.Add(user);

        return errors;
    }
    
    public User? Login(string login, string password)
    {
        var user = _userRepository.GetByLogin(login);

        if (user is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Success)
        {
            return user;
        }

        return null;
    }
    
    public List<ValidationError> Edit(
        int id,
        string name,
        string login,
        string email)
    {
        var errors = new List<ValidationError>();

        var user = _userRepository.GetById(id);

        if (user is null)
        {
            return errors;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(ValidationError.NameEmpty);
        }

        if (string.IsNullOrWhiteSpace(login))
        {
            errors.Add(ValidationError.LoginEmpty);
        }
        else
        {
            if (login.Length < 3)
                errors.Add(ValidationError.LoginTooShort);

            if (login.Length > 16)
                errors.Add(ValidationError.LoginTooLong);

            var existingUser = _userRepository.GetByLogin(login);

            if (existingUser is not null && existingUser.Id != id)
            {
                errors.Add(ValidationError.LoginAlreadyExists);
            }
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(ValidationError.EmailEmpty);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        user.Name = name;
        user.Login = login;
        user.Email = email;

        _userRepository.Update(user);

        return errors;
    }
    
    public bool Delete(int id)
    {
        var user = _userRepository.GetById(id);

        if (user is null)
        {
            return false;
        }

        _userRepository.Delete(user);

        return true;
    }
}