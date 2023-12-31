using System.Reflection.Emit;
using Authentication_Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserID);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.ResetPasswordToken);
        builder.Property(u => u.ResetPasswordTokenExpiry);
        builder.Property(u => u.RoleID);

        //set time stamp data type 
        builder.Property(u => u.ResetPasswordTokenExpiry).HasColumnType("timestamp with time zone");

        // unique attributes accross the entire app
        builder.HasIndex(u => u.UserID).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasOne(u => u.Role)
             .WithMany(r => r.Users)
             .HasForeignKey(u => u.RoleID)
             .IsRequired();
    }

}