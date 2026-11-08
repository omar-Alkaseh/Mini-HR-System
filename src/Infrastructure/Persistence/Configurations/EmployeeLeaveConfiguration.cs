using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeLeaveConfiguration : IEntityTypeConfiguration<EmployeeLeave>
{
    public void Configure(EntityTypeBuilder<EmployeeLeave> builder)
    {
        builder.HasKey(e => e.EmployeeLeaveId);

        builder.HasIndex(e => e.EmployeeId, "IX_EmployeeLeaves_EmployeeId");

        builder.HasIndex(e => e.RecordedByUserId, "IX_EmployeeLeaves_RecordedByUserId");

        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_EmployeeLeaves_CreatedAt");

        builder.HasOne(d => d.Employee).WithMany()
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_EmployeeLeaves_Employee");

        builder.HasOne(d => d.RecordedByUser).WithMany()
            .HasForeignKey(d => d.RecordedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_EmployeeLeaves_RecordedByUser");
    }
}