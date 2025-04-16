using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LeaveRequestRepository : Repository<LeaveRequest>
{
    public LeaveRequestRepository(LeaveManagementContext context) : base(context) { }

    public async Task<bool> HasOverlappingLeaves(int employeeId, DateTime start, DateTime end)
        => await _context.LeaveRequests.AnyAsync(lr =>
            lr.EmployeeId == employeeId &&
            lr.StartDate <= end && lr.EndDate >= start &&
            lr.Status != Domain.Enums.LeaveStatus.Rejected);

    public async Task<int> GetAnnualDaysTaken(int employeeId, int year)
        => await _context.LeaveRequests
            .Where(lr => lr.EmployeeId == employeeId &&
                         lr.LeaveType == Domain.Enums.LeaveType.Annual &&
                         lr.Status == Domain.Enums.LeaveStatus.Approved &&
                         lr.StartDate.Year == year)
            .SumAsync(lr => (lr.EndDate - lr.StartDate).Days + 1);

}
