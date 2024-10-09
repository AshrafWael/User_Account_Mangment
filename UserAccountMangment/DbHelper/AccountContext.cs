using HospitalManagmentSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace UserAccountMangment.DbHelper
{
    public class AccountContext :IdentityDbContext<ApplicationUser>
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
