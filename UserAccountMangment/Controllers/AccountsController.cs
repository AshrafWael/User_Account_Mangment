using HospitalManagmentSystem.BLL.Dtos.AccountDtos;
using HospitalManagmentSystem.BLL.Manager.Accounts;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserAccountMangment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IEmailSender _emailSender;

        public AccountsController(IAccountManager accountManager,IEmailSender emailSender)
        {
            _accountManager = accountManager;
            _emailSender = emailSender;

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
                await _emailSender.SendEmailAsync(registerDto.Email,
                   "<h1>Welcome to AuthApp", "Thank you for registering!</h1><p> new loign to your account at "
                   + DateTime.Now + "</p>");
                return Ok(result);
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AccountLoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountManager.Login(loginDto);
                if (result == null)
                {
                    return Unauthorized();
                }
                await _emailSender.SendEmailAsync(loginDto.Email,
                    "<h1>Welcome to AuthApp", "Thank you for registering!</h1><p> new loign to your account at "
                    + DateTime.Now + "</p>");
                return Ok(result);
            }
            return BadRequest();
        }
    }
}

