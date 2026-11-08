using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.HasIndex(e => e.CreatedByUserId, "IX_Employee_CreatedByUserId");

        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_Employee_CreatedAt");
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.IsActive).HasDefaultValue(true, "DF_Employee_IsActive");
        builder.Property(e => e.UpdatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_Employee_UpdatedAt");

        builder.HasOne(d => d.CreatedByUser).WithMany()
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Employee_CreatedByUser");
    }
}