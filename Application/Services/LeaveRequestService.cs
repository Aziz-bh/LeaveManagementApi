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

    public async Task<List<LeaveRequestDto>> GetFilteredAsync(LeaveRequestFilterDto filter)
    {
        var query = await _repository.GetAllAsync();
        var filtered = query.AsQueryable();

        if (filter.EmployeeId.HasValue)
            filtered = filtered.Where(l => l.EmployeeId == filter.EmployeeId.Value);

        if (filter.LeaveType.HasValue)
            filtered = filtered.Where(l => l.LeaveType == filter.LeaveType.Value);

        if (filter.Status.HasValue)
            filtered = filtered.Where(l => l.Status == filter.Status.Value);

        if (filter.StartDate.HasValue)
            filtered = filtered.Where(l => l.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            filtered = filtered.Where(l => l.EndDate <= filter.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
            filtered = filtered.Where(l => l.Reason.ToLower().Contains(filter.Keyword.ToLower()));

        // Sorting
        var sortBy = filter.SortBy;
        var sortOrder = filter.SortOrder.ToLower();

        if (sortOrder == "desc")
            filtered = filtered.OrderByDescending(e => EF.Property<object>(e, sortBy));
        else
            filtered = filtered.OrderBy(e => EF.Property<object>(e, sortBy));

        // Pagination
        filtered = filtered.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);

        return _mapper.Map<List<LeaveRequestDto>>(filtered.ToList());
    }
}
