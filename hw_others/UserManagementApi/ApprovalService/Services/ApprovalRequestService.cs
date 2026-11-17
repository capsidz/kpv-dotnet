
using ApprovalService.Data;
using ApprovalService.Enums;
using ApprovalService.Models;
using ApprovalService.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ApprovalService.Services;

public class ApprovalRequestService : IApprovalRequestService
{
    private readonly AppDbContext _context;
    private readonly IUserManagementClient _userManagementClient;
    private readonly IDistributedCache _cache;
    private readonly IAiAnalysisService _aiAnalysisService;

    public ApprovalRequestService(AppDbContext context, IUserManagementClient userManagementClient, IDistributedCache cache, IAiAnalysisService aiAnalysisService)
    {
        _context = context;
        _userManagementClient = userManagementClient;
        _cache = cache;
        _aiAnalysisService = aiAnalysisService;
    }

    public async Task<ApprovalRequest> CreateAsync(CreateApprovalRequestDto dto)
    {
        var userExists = await _userManagementClient.UserExistsAsync(dto.UserId);

        if (!userExists)
        {
            throw new KeyNotFoundException(
                $"User with ID {dto.UserId} not found.");
        }

        var aiResult = await _aiAnalysisService.AnalyzeAsync(dto.Title, dto.Description);

        var request = new ApprovalRequest
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Description = dto.Description,

            Status = ApprovalStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

            Summary = aiResult.Summary,
            Category = aiResult.Category,
            Priority = aiResult.Priority,
        };

        await _context.ApprovalRequests.AddAsync(request);
        await _context.SaveChangesAsync();

        await _cache.RemoveAsync("approval_requests:all");

        return request;
    }

    public async Task<List<ApprovalRequest>> GetAllAsync()
    {
        const string cacheKey = "approval_requests:all";

        var cachedRequests = await _cache.GetStringAsync(cacheKey);

        if (cachedRequests != null)
        {
            var requests = JsonSerializer.Deserialize<List<ApprovalRequest>>(
                cachedRequests);

            if (requests != null)
            {
                return requests;
            }
        }

        var dbRequests = await _context.ApprovalRequests
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var serializedRequests = JsonSerializer.Serialize(dbRequests);

        await _cache.SetStringAsync(
            cacheKey,
            serializedRequests,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });

        return dbRequests;
    }

    public async Task<ApprovalRequest?> GetByIdAsync(int id)
    {
        return await _context.ApprovalRequests
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<StatusUpdateResult> UpdateStatusAsync(
        int id,
        ApprovalStatus status)
    {
        var request = await _context.ApprovalRequests
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null)
        {
            return StatusUpdateResult.NotFound;
        }

        if (request.Status != ApprovalStatus.Pending)
        {
            return StatusUpdateResult.InvalidTransition;
        }

        if (status != ApprovalStatus.Approved &&
            status != ApprovalStatus.Rejected)
        {
            return StatusUpdateResult.InvalidTransition;
        }

        request.Status = status;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _cache.RemoveAsync("approval_requests:all");

        return StatusUpdateResult.Updated;
    }
}
