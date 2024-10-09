using HospitalManagmentSystem.BLL.Dtos.AccountDtos;
using HospitalManagmentSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace HospitalManagmentSystem.BLL.Manager.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AccountManager(UserManager<ApplicationUser> userManager ,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<GenralResponse> Login(AccountLoginDto LoginDto)
        {
            GenralResponse genralResponse = new GenralResponse();
            var user = await _userManager.FindByEmailAsync(LoginDto.Email);
            if (user == null)
            {
                return new GenralResponse
                {
                    Massegr = "there is no user with this email",
                    IsSuccess = false,
                };
            }

            else
            {
               var result = await _userManager
               .CheckPasswordAsync(user,LoginDto.Password);
                if (result == true) 
                {
                    //genrate Token
                    var claims= await _userManager
                        .GetClaimsAsync(user);
                     genralResponse= GenrateToken(claims);  
                    return genralResponse;
                }
                return new GenralResponse
                {
                    Massegr = "Invalid Password",
                    IsSuccess = false,
                };
               
            }
        }
        public async Task<GenralResponse> Register(AccountRegisterDto registerDto)
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto));
            GenralResponse genralResponse= new GenralResponse();    
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,  // Ensure this is set
                lasttName = registerDto.lasttName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
            };

            if (registerDto.Password != registerDto.ConfirmPassword)
                return new GenralResponse
                {
                    Massegr = "Password Dos not match password",
                    IsSuccess = false,
                };
            IdentityResult result = await _userManager
            .CreateAsync(user,registerDto.Password); //save over Db
            if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Email", registerDto.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                await _userManager.AddClaimsAsync(user, claims);
                genralResponse = GenrateToken(claims);
                return genralResponse;
            }
            else
            {
                return new GenralResponse
                {
                    Massegr = "User did not create",
                    IsSuccess = false,
                    Errors = result.Errors.Select(a => a.Description)
                };
            }
        }
            private GenralResponse GenrateToken(IList<Claim> claims)
            {
                var SecretKeyString = _configuration["AuthSettings:Key"];
                var SecretKeyByte = Encoding.ASCII.GetBytes(SecretKeyString);
                SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);

                SigningCredentials signingCredentials = new
                SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var ExpireDate = DateTime.Now.AddDays(3);

                JwtSecurityToken jwtSecurity = 
                new JwtSecurityToken(
                     issuer: _configuration["AuthSettings:issuer"],
                     audience: _configuration["AuthSettings:Audince"],
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: ExpireDate
                    );
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                string token = handler.WriteToken(jwtSecurity);
                return new GenralResponse
                { 
                    Massegr = "Taken As String",
                     Token = token,
                    ExpirationDate = ExpireDate
                };
            }
    }
}
