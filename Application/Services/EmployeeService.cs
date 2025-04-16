using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
        => _mapper.Map<List<EmployeeDto>>(await _repository.GetAllAsync());

    public async Task<EmployeeDto?> GetByIdAsync(int id)
        => _mapper.Map<EmployeeDto>(await _repository.GetByIdAsync(id));

    public async Task<EmployeeDto> CreateAsync(EmployeeDto dto)
    {
        var entity = _mapper.Map<Employee>(dto);
        await _repository.AddAsync(entity);
        return _mapper.Map<EmployeeDto>(entity);
    }

    public async Task UpdateAsync(int id, EmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");

        _mapper.Map(dto, employee);
        await _repository.UpdateAsync(employee);
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Employee not found.");

        await _repository.DeleteAsync(employee);
    }
}
