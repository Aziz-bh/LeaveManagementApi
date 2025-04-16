using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Enums;

namespace Application.Interfaces;
public interface ILeaveRequestService
{
    Task<List<LeaveRequestDto>> GetAllAsync();
    Task<LeaveRequestDto> GetByIdAsync(int id);
    Task<LeaveRequestDto> CreateAsync(LeaveRequestDto dto);
    Task UpdateAsync(int id, LeaveRequestDto dto);
    Task DeleteAsync(int id);
    Task ApproveAsync(int id);
    Task<List<LeaveRequestDto>> GetFilteredAsync(int? employeeId, LeaveType? leaveType, LeaveStatus? status, DateTime? startDate, DateTime? endDate, string? keyword, int page, int pageSize, string sortBy, string sortOrder);
    Task<object> GetReportAsync(int year);
}