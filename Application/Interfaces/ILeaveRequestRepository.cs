using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;
public interface ILeaveRequestRepository : IRepository<LeaveRequest>
{
    Task<bool> HasOverlappingLeaves(int employeeId, DateTime start, DateTime end);
    Task<int> GetAnnualDaysTaken(int employeeId, int year);
}