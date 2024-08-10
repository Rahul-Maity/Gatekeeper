using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Gatekeeper.Handler;

public  class LoginHandler()
{
    public static IResult HandleLogin(LoginForm loginForm)
    {
        var claims = new List<Claim>
        {
            new Claim("user_id", Guid.NewGuid().ToString()),
            new Claim("username", loginForm.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        return Results.SignIn(claimsPrincipal, authProperties, "cookie");
    }
}
