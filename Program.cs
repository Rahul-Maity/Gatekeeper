using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

const string authScheme = "cookie";
builder.Services.AddAuthentication(authScheme)
    .AddCookie(authScheme);

var app = builder.Build();



app.UseAuthentication();

app.MapGet("/username", (HttpContext ctx) =>
{

    return ctx.User.FindFirst("usr").Value ?? "empty";
});


//auth endpoint

app.MapGet("/sweden", (HttpContext ctx) =>
{
    if(!ctx.User.Identities.Any(x => x.AuthenticationType == authScheme))
    {
        ctx.Response.StatusCode = 401;
        return "";
    }

    if(!ctx.User.HasClaim("passport_type","eur"))
    {
        ctx.Response.StatusCode = 403;
        return "";
    }

    return "allowed";

   
});


app.MapGet("/login",async (HttpContext ctx) =>
{

    var claims = new List<Claim>();
    claims.Add(new Claim("usr","anton"));
    claims.Add(new Claim("passport_type", "eur"));
    var identity = new ClaimsIdentity(claims,authScheme);
    var user = new ClaimsPrincipal(identity);
    await ctx.SignInAsync(authScheme,user);
    return "ok";
});

app.UseHttpsRedirection();  



app.Run();




