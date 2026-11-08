using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.UserId);

        builder.ToTable("User");

        builder.HasIndex(e => e.Email, "UQ_UserAccount_Email").IsUnique();

        builder.Property(e => e.CreatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_AdminAccount_CreatedAt");
        builder.Property(e => e.Email).HasMaxLength(254);
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.IsActive).HasDefaultValue(true, "DF_AdminAccount_IsActive");
        builder.Property(e => e.PasswordHash).HasMaxLength(512);
        builder.Property(e => e.UpdatedAt)
            .HasPrecision(3)
            .HasDefaultValueSql("(sysdatetime())", "DF_AdminAccount_UpdatedAt");
    }
}