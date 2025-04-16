using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class LeaveManagementContext : DbContext
{
    public LeaveManagementContext(DbContextOptions<LeaveManagementContext> options)
        : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.LeaveRequests)
            .WithOne(lr => lr.Employee)
            .HasForeignKey(lr => lr.EmployeeId);

        base.OnModelCreating(modelBuilder);
    }
}

