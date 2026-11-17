using Microsoft.Extensions.Logging;
using UserManagementApi.Enums;
using UserManagementApi.Validation;
using UserManagementApi.Repository;
using UserManagementApi.Entities;

namespace UserManagementApi.Services;

public class UserService
{
    protected readonly UserRepository _userRepository;
    private readonly DateFilteredRequestValidator _validator;
    private readonly ILogger<UserService> _logger;

    public UserService(UserRepository userRepository, DateFilteredRequestValidator validator, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _validator = validator;
        _logger = logger;
    }

    public IEnumerable<User> GetFilteredUsers(DateFilterType? dateFilterType, DateTime from, DateTime? to)
    {
        if (!_validator.Validate(dateFilterType, from, to))
        {
            _logger.LogWarning("Invalid date filter parameters: DateFilterType={DateFilterType}, From={From}, To={To}", dateFilterType, from, to);
            throw new ArgumentException("Invalid date filter parameters.");
        }

        var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        var toUtc = DateTime.SpecifyKind(to ?? DateTime.UtcNow, DateTimeKind.Utc);

        return _userRepository.GetFiltered(dateFilterType.Value, fromUtc, toUtc);
    }

    public List<ValidationError> Edit(int id, string name, string login, string email)
    {
        var errors = new List<ValidationError>();

        var user = _userRepository.GetById(id);

        if (user is null)
        {
            _logger.LogWarning("User with ID {Id} not found for update", id);
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
            {
                errors.Add(ValidationError.LoginTooShort);
            }

            if (login.Length > 16)
            {
                errors.Add(ValidationError.LoginTooLong);
            }

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
            _logger.LogWarning(
                "User with ID {Id} has validation errors. ErrorCount={ErrorCount}",
                id,
                errors.Count);
            return errors;
        }

        user.Name = name;
        user.Login = login;
        user.Email = email;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);

        _logger.LogInformation("User with ID {Id} updated successfully", id);

        return errors;
    }
    
    public bool Delete(int id)
    {
        var user = _userRepository.GetById(id);

        if (user is null)
        {
            _logger.LogWarning("User with ID {Id} not found for delete", id);
            return false;
        }

        _userRepository.Delete(user);

        _logger.LogInformation("User with ID {Id} deleted successfully", id);
        return true;
    }

    public User GetUserById(int id)
    {
        var user = _userRepository.GetById(id);

        if (user is null)
        {
            _logger.LogWarning("User with ID {Id} not found", id);
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        _logger.LogInformation("User with ID {Id} found successfully", id);
        return user;
    }
}
