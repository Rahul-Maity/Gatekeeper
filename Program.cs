using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<authService>();

var app = builder.Build();

app.Use(async
    (ctx, next) =>
{
    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
    if(ctx.Request.Path.StartsWithSegments("/login"))
    {
        await next();
        return;
    }
    if (!ctx.Request.Cookies.TryGetValue("auth", out var authCookie))
    {
        ctx.Response.StatusCode = 401;
        ctx.Response.WriteAsync("NO auth cookie found");
        return;
    }

    var protector = idp.CreateProtector("auth-cookie");
    try
    {
        var unProtectedValue = protector.Unprotect(authCookie);
        var parts = unProtectedValue.Split(':');

        if (parts.Length != 2 || parts[0] != "usr")
        {
            ctx.Response.StatusCode = 401; // Unauthorized
            await ctx.Response.WriteAsync("Invalid auth cookie");
            return;
        }
        var key = parts[0];
        var value = parts[1];

        var claims = new List<Claim>();
        claims.Add(new Claim(key, value));
        var identity = new ClaimsIdentity(claims);
        ctx.User = new ClaimsPrincipal(identity);




    }
    catch
    {
        ctx.Response.StatusCode = 401; // Unauthorized
        await ctx.Response.WriteAsync("Invalid auth cookie");
        return;
    }


    //ctx.User = new ClaimsPrincipal()
    await next();
    

});

app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{

    return ctx.User.FindFirst("usr").Value;
});


app.MapGet("/login", (authService auth) =>
{
    auth.signin(); 
    return "ok";
});

app.UseHttpsRedirection();  



app.Run();


public class authService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IDataProtectionProvider _idp;

    public authService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
    {
        _idp = idp;
        _accessor = accessor;
    }

    public void signin()
    {
        var protector = _idp.CreateProtector("auth-cookie");
        _accessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:anton")}";
    }

}

