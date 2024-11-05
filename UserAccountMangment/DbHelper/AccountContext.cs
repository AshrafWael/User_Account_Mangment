using HospitalManagmentSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using UserAccountMangment.Models;

namespace UserAccountMangment.DbHelper
{
    public class AccountContext :IdentityDbContext<ApplicationUser>
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserPermission>  userPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserPermission>().ToTable("UserPermission")
                .HasKey(X=> new { X.UserId ,X.PermissionId });

            base.OnModelCreating(builder);
        }

    }
}
