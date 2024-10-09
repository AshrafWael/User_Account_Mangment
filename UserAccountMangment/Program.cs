
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

namespace UserAccountMangment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddDbContext<AccountContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.
                    GetConnectionString("Cs"));
            });
            builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity
            .IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequireLowercase = false;
                Options.Password.RequireUppercase = true;
                Options.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<AccountContext>();

            
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
                    ValidateAudience = true,
                    ValidAudience =builder.Configuration["AuthSettings:Audince"],
                    ValidIssuer = builder.Configuration["AuthSettings:issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration["AuthSettings:Key"])),
                    ValidateIssuerSigningKey = true
                };
            });


            builder.Services.AddTransient<Manager.Accounts.IEmailSender, EmailSender>(sp =>
                new EmailSender(builder.Configuration["SendGridApiKey"]));


            #region configer Authentcation an token 
            //builder.Services.AddAuthentication(Options =>
            //{
            //    Options.DefaultAuthenticateScheme = "JWT"; //Validate on token
            //    Options.DefaultChallengeScheme = "JWT"; //redirect on login again if not Autantkated
            //}).AddJwtBearer("JWT", Options =>
            //{
            //    var SecretKeyString = builder.Configuration.GetValue<string>("SecretKey");
            //    var SecretKeyByte = Encoding.ASCII.GetBytes(SecretKeyString);
            //    SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);
            //    Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens
            //    .TokenValidationParameters()
            //    {
            //        IssuerSigningKey = securityKey,
            //        ValidateIssuer = false, // who Resive token
            //        ValidateAudience = false,//FRONTEND who send token
            //    };
            //});

            #endregion















            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
