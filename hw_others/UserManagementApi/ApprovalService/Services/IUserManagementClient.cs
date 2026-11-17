using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApprovalService.Services;

public interface IUserManagementClient
{
    Task<bool> UserExistsAsync(int userId);
}