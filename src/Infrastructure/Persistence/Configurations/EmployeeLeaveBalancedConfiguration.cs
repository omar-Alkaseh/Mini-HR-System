using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeLeaveBalancedConfiguration : IEntityTypeConfiguration<EmployeeLeaveBalance>
{
    public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
    {
        builder.HasKey(e => e.BalanceId);

        builder.ToTable("EmployeeLeaveBalanced");

        builder.HasIndex(e => new { e.EmployeeId, e.LeaveYear }, "UQ_EmployeeLeaveBalanced_EmployeeId_LeaveYear").IsUnique();

        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_EmployeeLeaveBalanced_CreatedAt");
        builder.Property(e => e.EntitledDays).HasColumnType("decimal(4, 1)");
        builder.Property(e => e.LeaveYear).HasDefaultValueSql("(datepart(year,sysdatetime()))", "DF_EmployeeLeaveBalanced_LeaveYear");
        builder.Property(e => e.UpdatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_EmployeeLeaveBalanced_UpdatedAt");

        builder.HasOne(d => d.Employee).WithMany()
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_EmployeeLeaveBalanced_Employee");
    }
}