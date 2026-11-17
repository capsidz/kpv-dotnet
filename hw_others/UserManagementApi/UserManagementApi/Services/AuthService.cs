using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserManagementApi.Entities;
using UserManagementApi.Repository;

namespace UserManagementApi.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserRepository userRepository, PasswordHasher<User> passwordHasher, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public User? Login(string login, string password)
    {
        var user = _userRepository.GetByLogin(login);

        if (user is null)
        {
            _logger.LogWarning("Login failed for {Login}", login);   
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Success)
        {
            _logger.LogInformation("User {Login} logged in successfully.", login);
            return user;
        }

        _logger.LogWarning("Login failed for {Login}", login);
        return null;
    }
}
