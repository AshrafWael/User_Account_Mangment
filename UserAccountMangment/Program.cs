
using HospitalManagmentSystem.BLL.Manager.Accounts;
using HospitalManagmentSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using UserAccountMangment.DbHelper;
using UserAccountMangment.Dtos.AccountDtos;
using UserAccountMangment.Manager.Accounts;
using UserAccountMangment.ConfigrationOptions;
using UserAccountMangment.Models;
using Microsoft.Extensions.Options;
using UserAccountMangment.Authentication;

namespace UserAccountMangment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
             builder.Services.AddControllers();
            builder.Services.AddControllers(Options =>
            Options.Filters.Add<PermissionBasedAuthenticationFilter>()
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IAccountManager, AccountManager>();

            // builder.Services.AddTransient<Manager.Accounts.IEmailSender, EmailSender>(sp =>
            //  new EmailSender(builder.Configuration["SendGridApiKey"]));
            var JwtOptions = builder.Configuration.GetSection("AuthSettings").Get<JwtOptions>();
            builder.Services.AddSingleton(JwtOptions);
            #endregion
            #region Coniction String configration
            builder.Services.AddDbContext<AccountContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.
                    GetConnectionString("Cs"));
            });
            #endregion
            #region Identity Configartions
            builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity
            .IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequireLowercase = false;
                Options.Password.RequireUppercase = true;
                Options.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<AccountContext>();
            #endregion
            #region JwtBerer Token Confgartions
            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
            {
                Options.SaveToken = true;
                Options.RequireHttpsMetadata = true;
                Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens
                .TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtOptions.issuer,
                    ValidateAudience = true,
                    ValidAudience =JwtOptions.Audince,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration["AuthSettings:Key"]))
                };
            });
            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
