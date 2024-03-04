using JWT.Mapping;
using JWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT.Data
{
    public class MyContext : IdentityDbContext<User, Role, int,
                             IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                             IdentityRoleClaim<int>, IdentityUserToken<int>>

    {
        public DbSet<UserRole> UserRole { get; set; }    
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserRoleMapping());
        }

    }
}
