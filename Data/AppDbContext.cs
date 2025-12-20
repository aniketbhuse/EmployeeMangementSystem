using EmployeeMangementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMangementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Employee>().HasNoKey(); // Not typical for regular models
            modelBuilder.Entity<Employee>().HasKey(e => e.EmpId);
            modelBuilder.Entity<EmployeeAppraisal>().HasKey(e => e.AppraisalId);
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<EmployeeAppraisal> EmployeeAppraisal { get;  set; }
    }
}
