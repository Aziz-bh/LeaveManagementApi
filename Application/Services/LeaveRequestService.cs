using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class LeaveRequestService : ILeaveRequestService
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IMapper _mapper;

    public LeaveRequestService(ILeaveRequestRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<LeaveRequestDto>> GetAllAsync()
    {
        var leaves = await _repository.GetAllAsync();
        return _mapper.Map<List<LeaveRequestDto>>(leaves);
    }

    public async Task<LeaveRequestDto?> GetByIdAsync(int id)
    {
        var leave = await _repository.GetByIdAsync(id);
        return _mapper.Map<LeaveRequestDto>(leave);
    }

    public async Task<LeaveRequestDto> CreateAsync(LeaveRequestDto dto)
    {
        if (await _repository.HasOverlappingLeaves(dto.EmployeeId, dto.StartDate, dto.EndDate))
            throw new InvalidOperationException("Overlapping leave dates for this employee.");

        if (dto.LeaveType == LeaveType.Annual)
        {
            var annualDaysTaken = await _repository.GetAnnualDaysTaken(dto.EmployeeId, dto.StartDate.Year);
            var requestedDays = (dto.EndDate - dto.StartDate).Days + 1;
            if (annualDaysTaken + requestedDays > 20)
                throw new InvalidOperationException("Exceeded 20 annual leave days.");
        }

        if (dto.LeaveType == LeaveType.Sick && string.IsNullOrWhiteSpace(dto.Reason))
            throw new InvalidOperationException("Sick leave requires a reason.");

        dto.Status = LeaveStatus.Pending;
        dto.CreatedAt = DateTime.UtcNow;

        var entity = _mapper.Map<LeaveRequest>(dto);
        await _repository.AddAsync(entity);

        return _mapper.Map<LeaveRequestDto>(entity);
    }

    public async Task UpdateAsync(int id, LeaveRequestDto dto)
    {
        var leave = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Leave request not found.");

        // Optional: add validations again if needed
        _mapper.Map(dto, leave);
        await _repository.UpdateAsync(leave);
    }

    public async Task DeleteAsync(int id)
    {
        var leave = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Leave request not found.");

        await _repository.DeleteAsync(leave);
    }

    public async Task ApproveAsync(int id)
    {
        var leave = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Leave request not found.");

        if (leave.Status != LeaveStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved.");

        leave.Status = LeaveStatus.Approved;
        await _repository.UpdateAsync(leave);
    }

    public async Task<List<LeaveRequestDto>> GetFilteredAsync(
        int? employeeId, LeaveType? leaveType, LeaveStatus? status,
        DateTime? startDate, DateTime? endDate, string? keyword,
        int page, int pageSize, string sortBy, string sortOrder)
    {
        var query = await _repository.GetAllAsync();
        var filtered = query.AsQueryable();

        if (employeeId.HasValue)
            filtered = filtered.Where(l => l.EmployeeId == employeeId.Value);

        if (leaveType.HasValue)
            filtered = filtered.Where(l => l.LeaveType == leaveType.Value);

        if (status.HasValue)
            filtered = filtered.Where(l => l.Status == status.Value);

        if (startDate.HasValue)
            filtered = filtered.Where(l => l.StartDate >= startDate.Value);

        if (endDate.HasValue)
            filtered = filtered.Where(l => l.EndDate <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
            filtered = filtered.Where(l => l.Reason.ToLower().Contains(keyword.ToLower()));

        filtered = sortOrder?.ToLower() == "desc"
            ? filtered.OrderByDescending(e => EF.Property<object>(e, sortBy))
            : filtered.OrderBy(e => EF.Property<object>(e, sortBy));

        var result = filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return _mapper.Map<List<LeaveRequestDto>>(result);
    }

    public async Task<object> GetReportAsync(int year)
    {
        var all = await _repository.GetAllAsync();
        var filtered = all.Where(l => l.StartDate.Year == year && l.Status == LeaveStatus.Approved);

        var report = filtered
            .GroupBy(l => l.Employee.FullName)
            .Select(g => new
            {
                Employee = g.Key,
                TotalLeaves = g.Count(),
                AnnualLeaves = g.Count(l => l.LeaveType == LeaveType.Annual),
                SickLeaves = g.Count(l => l.LeaveType == LeaveType.Sick)
            })
            .ToList();

        return report;
    }
}
