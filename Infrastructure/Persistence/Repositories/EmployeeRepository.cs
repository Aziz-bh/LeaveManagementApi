using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class EmployeeRepository : Repository<Employee>
{
    public EmployeeRepository(LeaveManagementContext context) : base(context) { }
}
