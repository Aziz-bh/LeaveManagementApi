using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;
public interface ILeaveRequestRepository : IRepository<LeaveRequest>
{
    Task<bool> HasOverlappingLeaves(int employeeId, DateTime start, DateTime end);
    Task<int> GetAnnualDaysTaken(int employeeId, int year);
    Task<List<LeaveReportDto>> GetLeaveReportAsync(int year, string? department, DateTime? startDate, DateTime? endDate);

}