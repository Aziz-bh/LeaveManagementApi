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
}
