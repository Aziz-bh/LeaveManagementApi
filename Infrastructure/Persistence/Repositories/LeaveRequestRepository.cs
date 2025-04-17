using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LeaveRequestRepository : Repository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestRepository(LeaveManagementContext context) : base(context)
    {
    }

    public async Task<bool> HasOverlappingLeaves(int employeeId, DateTime start, DateTime end)
    {
        return await _context.LeaveRequests
            .AnyAsync(lr =>
                lr.EmployeeId == employeeId &&
                lr.Status != LeaveStatus.Rejected &&
                lr.StartDate <= end &&
                lr.EndDate >= start);
    }

    public async Task<int> GetAnnualDaysTaken(int employeeId, int year)
    {
        return await _context.LeaveRequests
            .Where(lr =>
                lr.EmployeeId == employeeId &&
                lr.LeaveType == LeaveType.Annual &&
                lr.Status == LeaveStatus.Approved &&
                lr.StartDate.Year == year)
            .SumAsync(lr => (lr.EndDate - lr.StartDate).Days + 1);
    }

    public async Task<List<LeaveReportDto>> GetLeaveReportAsync(int year, string? department, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Status == LeaveStatus.Approved && lr.StartDate.Year == year);

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(lr => lr.Employee.Department == department);

        if (startDate.HasValue)
            query = query.Where(lr => lr.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(lr => lr.EndDate <= endDate.Value);

        return await query
            .GroupBy(lr => new { lr.Employee.FullName, lr.Employee.Department })
            .Select(g => new LeaveReportDto
            {
                EmployeeName = g.Key.FullName,
                Department = g.Key.Department,
                TotalLeaves = g.Count(),
                AnnualLeaves = g.Count(lr => lr.LeaveType == LeaveType.Annual),
                SickLeaves = g.Count(lr => lr.LeaveType == LeaveType.Sick)
            })
            .ToListAsync();
    }

}
