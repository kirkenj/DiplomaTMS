using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Interfaces
{
    public interface IAppDBContext
    {
        DbSet<User> Contracts { get; }
        DbSet<Role> Roles { get; }
        DbSet<User> Users { get; }
        DbSet<Contract> MonthReports { get; }
        DbSet<Department> Departments { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
