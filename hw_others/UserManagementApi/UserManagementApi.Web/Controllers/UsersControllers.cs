using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Enums;
using UserManagementApi.Services;
using UserManagementApi.Validation;
using UserManagementApi.Web.Models;

namespace UserManagementApi.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RegistrationService _registrationService;
    private readonly AuthService _authService;
    private readonly UserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(RegistrationService registrationService, AuthService authService, 
    UserService userService, ILogger<UsersController> logger)
    {
        _registrationService = registrationService;
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for login {Login}", request.Login);

        var errors = _registrationService.Register(
            request.Name,
            request.Login,
            request.Email,
            request.Password);

        if (errors.Count > 0)
        {
            _logger.LogWarning(
                "Registration failed for login {Login}. Validation errors: {ErrorCount}",
                request.Login,
                errors.Count);

            return BadRequest(errors);
        }

        _logger.LogInformation(
            "User successfully registered with login {Login}",
            request.Login);

        return Ok("User successfully registered.");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        _logger.LogInformation(
            "Login attempt for login {Login}",
            request.Login);

        var user = _authService.Login(
            request.Login,
            request.Password);

        if (user is null)
        {
            _logger.LogWarning(
                "Login failed for login {Login}",
                request.Login);

            return Unauthorized("Invalid login or password.");
        }


        _logger.LogInformation(
            "User successfully logged in with login {Login} and Id {UserId}",
            user.Login,
            user.Id);

        return Ok(new
        {
            user.Id,
            user.Name,
            user.Login,
            user.Email
        });
    }

    [HttpPut("{id}")]
    public IActionResult Edit(
        int id,
        EditUserRequest request)
    {
        _logger.LogInformation("User update attempt for user Id {UserId}", id);

        var errors = _userService.Edit(
            id,
            request.Name,
            request.Login,
            request.Email);

        if (errors.Count > 0)
        {
            _logger.LogWarning(
                "User update failed for user Id {UserId}. Validation errors: {ErrorCount}",
                id,
            errors.Count);

            return BadRequest(errors);
        }


        _logger.LogInformation(
            "User successfully updated with Id {UserId}",
            id);

        return Ok("User successfully updated.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation(
            "User deletion attempt for user Id {UserId}",
            id);

        var deleted = _userService.Delete(id);

        if (!deleted)
        {
            _logger.LogWarning(
                "User deletion failed. User with Id {UserId} was not found",
                id);

            return NotFound("User not found.");
        }

        _logger.LogInformation(
            "User successfully deleted with Id {UserId}",
            id);

        return Ok("User successfully deleted.");
    }

    [HttpGet("filtered")]
    public IActionResult GetFilteredUsers([FromQuery] DateFilteredRequest request)
    {
        _logger.LogInformation(
            "Getting users with date filter {DateFilterType} from {From} to {To}",
            request.DateFilterType,
            request.From,
            request.To);

        try
        {
            var users = _userService.GetFilteredUsers(request.DateFilterType, request.From, request.To);
            var userResponses = users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Login = u.Login,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            _logger.LogInformation(
                "Users filtered successfully. Found {UserCount} users",
                userResponses.Count);

            return Ok(userResponses);
        }

        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid parameters for user filtering");

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        _logger.LogInformation(
            "Getting user by Id {UserId}",
            id);

        try
        {
            var user = _userService.GetUserById(id);

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(userResponse);
        }
        
        catch (KeyNotFoundException)
        {
            return NotFound("User not found.");
        }
    }
}