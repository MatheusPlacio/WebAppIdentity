using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using JWT.Models;

namespace JWT.Mapping
{
    public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId});
            builder.HasOne(x => x.Role).WithMany(x => x.UserRole).HasForeignKey(x => x.RoleId).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.UserRole).HasForeignKey(x => x.UserId).IsRequired();
        }
    }
}
