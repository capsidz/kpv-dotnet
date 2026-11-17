using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApprovalService.Models.Dtos;

namespace ApprovalService.Services;
public interface IAiAnalysisService
{
    Task<AiAnalysisResultDto> AnalyzeAsync(string title, string description);
}