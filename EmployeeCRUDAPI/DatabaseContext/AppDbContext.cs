using EmployeeCRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace EmployeeCRUDAPI.DatabbaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }   

        public DbSet<Employee> Employees { get; set; }
    }
}
