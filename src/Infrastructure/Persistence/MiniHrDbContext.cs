using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public partial class MiniHrDbContext : DbContext
{
    public MiniHrDbContext(DbContextOptions<MiniHrDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

    public virtual DbSet<EmployeeLeaveBalance> EmployeeLeaveBalanced { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniHrDbContext).Assembly);
    }
}
