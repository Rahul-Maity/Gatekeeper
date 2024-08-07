﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gatekeeper.Controllers
{
   
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("/login")]
        public IActionResult Login() =>
            SignIn(new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),
                        new Claim("my_role_claim","admin")
                    },
                    "cookie",
                    nameType:null,
                    roleType:"my_role_claim"
                   )

                ),
                authenticationScheme: "cookie"
              );
    }
}
