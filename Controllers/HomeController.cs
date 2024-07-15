using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gatekeeper.Controllers
{
    
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpPost("/mvc/login")]
        public async Task<IActionResult> Login()
        {
                await HttpContext.SignInAsync("Default",new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
                        }, "Default"

                        )


                    ));

                return Ok();
        }

    }
}
