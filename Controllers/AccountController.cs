using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gatekeeper.Controllers
{

    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("/login")]
        public async Task<IActionResult> Login(SignInManager<IdentityUser> signInManager)
        {
           await  signInManager.PasswordSignInAsync("test@test.com", "password", false, false);
            return Ok();
        }
    }
}
