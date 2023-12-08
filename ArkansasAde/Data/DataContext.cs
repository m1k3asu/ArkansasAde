using ArkansasAde.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectArkansasAde.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbOptions) : base(dbOptions)
        {
            
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Department> Departments => Set<Department>();
    }
}
