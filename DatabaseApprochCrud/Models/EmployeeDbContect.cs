using Microsoft.EntityFrameworkCore;

namespace DatabaseApprochCrud.Models
{
    public class EmployeeDbContect : DbContext
    {
        public EmployeeDbContect(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; } 
    }
}
