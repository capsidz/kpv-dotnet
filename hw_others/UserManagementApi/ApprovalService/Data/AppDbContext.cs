using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApprovalService.Models;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
}
