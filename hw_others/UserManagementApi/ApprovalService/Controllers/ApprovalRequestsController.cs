
using ApprovalService.Enums;
using ApprovalService.Models.Dtos;
using ApprovalService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalService.Controllers;

[ApiController]
[Route("api/requests")]
public class ApprovalRequestsController : ControllerBase
{
    private readonly IApprovalRequestService _service;

    public ApprovalRequestsController(IApprovalRequestService service)
    {
        _service = service;
    }

    // api/requests
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApprovalRequestDto dto)
    {
        try
        {
            var request = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = request.Id },
                request);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // api/requests
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _service.GetAllAsync();

        return Ok(requests);
    }

    // GET: api/requests/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var request = await _service.GetByIdAsync(id);

        if (request == null)
        {
            return NotFound(new { message = "Request not found" });
        }

        return Ok(request);
    }

    // api/requests/5/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateApprovalStatusDto dto)
    {
        var result = await _service.UpdateStatusAsync(id, dto.Status);

        return result switch
        {
            StatusUpdateResult.Updated => NoContent(),

            StatusUpdateResult.NotFound =>
                NotFound(new { message = "Request not found" }),

            StatusUpdateResult.InvalidTransition =>
                Conflict(new { message = "Invalid status transition" }),

            _ => StatusCode(500)
        };
    }
}
