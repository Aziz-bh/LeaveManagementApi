using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class LeaveManagementContext : DbContext
{
    public LeaveManagementContext(DbContextOptions<LeaveManagementContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FullName = "Aziz Ben Hmida", Department = "Informatique", JoiningDate = new DateTime(2020, 1, 10) },
            new Employee { Id = 2, FullName = "Ahmed Gharbi", Department = "Comptabilité", JoiningDate = new DateTime(2019, 6, 15) },
            new Employee { Id = 3, FullName = "Ons Trabelsi", Department = "Ressources Humaines", JoiningDate = new DateTime(2021, 3, 5) }
        );

        modelBuilder.Entity<LeaveRequest>().HasData(
            new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveType = LeaveType.Annual,
                StartDate = new DateTime(2023, 7, 1),
                EndDate = new DateTime(2023, 7, 5),
                Status = LeaveStatus.Approved,
                Reason = "Vacances à Hammamet",
                CreatedAt = new DateTime(2023, 6, 15)
            },
            new LeaveRequest
            {
                Id = 2,
                EmployeeId = 1,
                LeaveType = LeaveType.Sick,
                StartDate = new DateTime(2023, 9, 12),
                EndDate = new DateTime(2023, 9, 14),
                Status = LeaveStatus.Approved,
                Reason = "Grippe saisonnière",
                CreatedAt = new DateTime(2023, 9, 10)
            },
            new LeaveRequest
            {
                Id = 3,
                EmployeeId = 2,
                LeaveType = LeaveType.Annual,
                StartDate = new DateTime(2023, 8, 1),
                EndDate = new DateTime(2023, 8, 10),
                Status = LeaveStatus.Rejected,
                Reason = "Voyage longue durée",
                CreatedAt = new DateTime(2023, 7, 20)
            },
            new LeaveRequest
            {
                Id = 4,
                EmployeeId = 2,
                LeaveType = LeaveType.Other,
                StartDate = new DateTime(2023, 10, 20),
                EndDate = new DateTime(2023, 10, 21),
                Status = LeaveStatus.Pending,
                Reason = "Urgence familiale",
                CreatedAt = new DateTime(2023, 10, 18)
            },
            new LeaveRequest
            {
                Id = 5,
                EmployeeId = 3,
                LeaveType = LeaveType.Sick,
                StartDate = new DateTime(2023, 5, 3),
                EndDate = new DateTime(2023, 5, 4),
                Status = LeaveStatus.Approved,
                Reason = "Extraction dentaire",
                CreatedAt = new DateTime(2023, 5, 1)
            }
        );
    }


}
