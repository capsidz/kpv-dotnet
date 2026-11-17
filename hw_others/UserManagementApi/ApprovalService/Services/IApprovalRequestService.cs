using ApprovalService.Enums;
using ApprovalService.Models;
using ApprovalService.Models.Dtos;

namespace ApprovalService.Services;

public interface IApprovalRequestService
{
    Task<ApprovalRequest> CreateAsync(CreateApprovalRequestDto dto);

    Task<List<ApprovalRequest>> GetAllAsync();

    Task<ApprovalRequest?> GetByIdAsync(int id);

    Task<StatusUpdateResult> UpdateStatusAsync(int id, ApprovalStatus status);
}