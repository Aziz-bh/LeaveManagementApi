using System;
using System.Globalization;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using LinqKit;

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

    public async Task<List<LeaveRequestDto>> GetFilteredAsync(LeaveRequestFilterDto filter)
    {
        var all = await _repository.GetAllAsync();
        var predicate = PredicateBuilder.New<LeaveRequest>(true);

        if (filter.EmployeeId.HasValue)
            predicate = predicate.And(l => l.EmployeeId == filter.EmployeeId.Value);

        if (filter.LeaveType.HasValue)
            predicate = predicate.And(l => l.LeaveType == filter.LeaveType.Value);

        if (filter.Status.HasValue)
            predicate = predicate.And(l => l.Status == filter.Status.Value);

        if (filter.StartDate.HasValue)
            predicate = predicate.And(l => l.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            predicate = predicate.And(l => l.EndDate <= filter.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
            predicate = predicate.And(l => l.Reason.ToLower().Contains(filter.Keyword.ToLower()));

        //Filtering (i used LinqKit for dynamic filtering)
        var filtered = all.AsQueryable().AsExpandable().Where(predicate);

   
        var propInfo = typeof(LeaveRequest).GetProperty(filter.SortBy);
        if (propInfo != null)
        {
            filtered = filter.SortOrder.ToLower() == "desc"
                ? filtered.OrderByDescending(e => propInfo.GetValue(e, null))
                : filtered.OrderBy(e => propInfo.GetValue(e, null));
        }

        // Pagination
        filtered = filtered
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);

        return _mapper.Map<List<LeaveRequestDto>>(filtered.ToList());
    }

    public async Task<List<LeaveReportDto>> GetReportAsync(int year, string? department, DateTime? startDate, DateTime? endDate)
    {
        return await _repository.GetLeaveReportAsync(year, department, startDate, endDate);
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


}
