using System;
using System.ComponentModel.DataAnnotations;
using ApprovalService.Enums;

namespace ApprovalService.Models;
public class ApprovalRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // property to store the AI analysis result as JSON
    [MaxLength(500)]
    public string Summary { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Priority { get; set; } = string.Empty;
}
