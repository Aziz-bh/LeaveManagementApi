using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(LeaveManagementContext context) : base(context) { }
}
