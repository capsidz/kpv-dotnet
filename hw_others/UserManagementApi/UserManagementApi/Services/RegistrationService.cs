using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(RegistrationValidator validator, UserRepository userRepository, 
    PasswordHasher<User> passwordHasher, ILogger<RegistrationService> logger)
    {
        _validator = validator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public List<ValidationError> Register(string name, string login, string email, string password)
    {
        var errors = _validator.Validate(name, login, email, password);

        if (errors.Count > 0)
        {
            _logger.LogWarning("Registration failed with validation errors: {ErrorCount}", errors.Count);
            return errors;
        }
        
        if (_userRepository.GetByLogin(login) is not null)
        {
            errors.Add(ValidationError.LoginAlreadyExists);
            _logger.LogWarning("Registration failed: login {Login} already exists", login);
            return errors;
        }
        
        var user = new User
        {
            Name = name,
            Login = login,
            Email = email,
            PasswordHash = "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        
        _userRepository.Add(user);

        _logger.LogInformation("User {Login} with ID: {UserId} registered successfully.", user.Login, user.Id);
        return errors;
    }
}