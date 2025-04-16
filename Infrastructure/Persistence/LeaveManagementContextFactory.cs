using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class LeaveManagementContextFactory : IDesignTimeDbContextFactory<LeaveManagementContext>
{
    public LeaveManagementContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LeaveManagementContext>();

        optionsBuilder.UseSqlite("Data Source=leaveManagement.db");

        return new LeaveManagementContext(optionsBuilder.Options);
    }
}
