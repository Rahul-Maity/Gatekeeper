using Microsoft.AspNetCore.DataProtection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

var app = builder.Build();


app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    if (!ctx.Request.Cookies.TryGetValue("auth", out var authCookie))
    {
        return "no auth cookie found";
    }

    var protector = idp.CreateProtector("auth-cookie");

   var unProtectedValue = protector.Unprotect(authCookie);
    var parts = unProtectedValue.Split(':');

    if (parts.Length != 2 || parts[0] != "usr")
    {
        return "invalid auth cookie";
    }

    var username = parts[1];
    return username;
});


app.MapGet("/login", (HttpContext ctx,IDataProtectionProvider idp) =>
{
   var protector = idp.CreateProtector("auth-cookie");
    ctx.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:anton")}";
    return "ok";
});

app.UseHttpsRedirection();



app.Run();

