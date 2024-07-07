using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace App.Controllers.Admin
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetUsersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetByUsername([FromQuery] string username)
        {
            return Ok(await _accountService.GetByUsernameAsync(username));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoles()
        {
            await _accountService.CreateRoleAsync();


            return Ok();
        }
    }
}
