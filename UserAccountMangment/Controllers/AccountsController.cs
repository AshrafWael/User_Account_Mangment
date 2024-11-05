using HospitalManagmentSystem.BLL.Dtos.AccountDtos;
using HospitalManagmentSystem.BLL.Manager.Accounts;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserAccountMangment.Authentication;
using UserAccountMangment.Models;

namespace UserAccountMangment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountManager _accountManager;


        public AccountsController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
          

        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(AccountRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager
                    .Register(registerDto);
                if (result == null)
                {
                    return BadRequest(result);
                }
          
                return Ok(result);
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("Login")]
        [Authorize]
        [CheckPermission(Permission.Admin)]
        public async Task<IActionResult> Login(AccountLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.Login(loginDto);
                if (result == null)
                {
                    return Unauthorized();
                }
        
                return Ok(result);
            }
            return BadRequest();
        }
    }
}

