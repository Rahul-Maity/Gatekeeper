

using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


#region Data-Protection

//below configuration aimed for sharing keys to use already setup cookie in localhost in other instances

var con = "127.0.0.1:5263";

var redis = ConnectionMultiplexer.Connect(con);


builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis,"DataProtection-Key")
    .SetApplicationName("unique");


#endregion

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.Cookie.Domain = ".company.local";
    });







var app =  builder.Build();

app.MapGet("/", () => "Hello world Identity");

//secure route

app.MapGet("/protected", () => "secure content").RequireAuthorization();




app.MapGet("login", (HttpContext ctx) =>
{
    ctx.SignInAsync(new ClaimsPrincipal(new[]
    {
        new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
        },CookieAuthenticationDefaults.AuthenticationScheme)
    }));



    return "ok";
});


app.UseAuthentication();
app.UseAuthorization();


app.Run();
