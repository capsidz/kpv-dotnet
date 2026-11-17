using System.ComponentModel.DataAnnotations;
using ApprovalService.Enums;

namespace ApprovalService.Models.Dtos;

public class UpdateApprovalStatusDto
{
    [EnumDataType(typeof(ApprovalStatus))]
    public ApprovalStatus Status { get; set; }
}