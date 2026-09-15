using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Services;
using UserManagementApi.Web.Models;

namespace UserManagementApi.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RegistrationService _service;

    public UsersController(RegistrationService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var errors = _service.Register(
            request.Name,
            request.Login,
            request.Email,
            request.Password);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        return Ok("User successfully registered.");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = _service.Login(
            request.Login,
            request.Password);

        if (user is null)
        {
            return Unauthorized("Invalid login or password.");
        }

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
        var errors = _service.Edit(
            id,
            request.Name,
            request.Login,
            request.Email);

        if (errors.Count > 0)
        {
            return BadRequest(errors);
        }

        return Ok("User successfully updated.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _service.Delete(id);

        if (!deleted)
        {
            return NotFound("User not found.");
        }

        return Ok("User successfully deleted.");
    }
}