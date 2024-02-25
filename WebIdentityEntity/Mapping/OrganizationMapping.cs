using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppIdentityEntity.Models;

namespace WebIdentityEntity.Mapping
{
    public class OrganizationMapping : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany<MyUser>()
                   .WithOne()
                   .HasForeignKey(x => x.OrgId)
                   .IsRequired(false);

        }
    }
}
