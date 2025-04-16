using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
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
        var requests = await _repository.GetAllAsync();
        return _mapper.Map<List<LeaveRequestDto>>(requests);
    }
    public async Task<LeaveRequestDto> GetByIdAsync(int id)
    {
        var request = await _repository.GetByIdAsync(id);
        return request == null ? null : _mapper.Map<LeaveRequestDto>(request);
    }
    public async Task<LeaveRequestDto> CreateAsync(LeaveRequestDto dto)
    {
        var request = _mapper.Map<LeaveRequest>(dto);
        await _repository.AddAsync(request);
        return _mapper.Map<LeaveRequestDto>(request);
    }
    public async Task UpdateAsync(int id, LeaveRequestDto dto)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request == null) throw new Exception("Leave request not found");
        _mapper.Map(dto, request);
        await _repository.UpdateAsync(request);
    }
    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
    public async Task ApproveAsync(int id)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request == null) throw new Exception("Leave request not found");
        if (request.Status != LeaveStatus.Pending) throw new Exception("Only pending requests can be approved");
        request.Status = LeaveStatus.Approved;
        await _repository.UpdateAsync(request);
    }
    public async Task<List<LeaveRequestDto>> GetFilteredAsync(int? employeeId, LeaveType? leaveType, LeaveStatus? status, DateTime? startDate, DateTime? endDate, string? keyword, int page, int pageSize, string sortBy, string sortOrder)
    {
        // Implementation for filtering, pagination, and sorting
        throw new NotImplementedException();
    }
    public async Task<object> GetReportAsync(int year)
    {
        // Implementation for reporting
        throw new NotImplementedException();
    }
}