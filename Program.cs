using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

const string authScheme = "cookie";

const string authScheme2 = "cookie2";

builder.Services.AddAuthentication(authScheme)
    .AddCookie(authScheme)
    .AddCookie(authScheme2);


builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("eu passport", pb =>
    {
        pb.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(authScheme)
        .AddRequirements()
        .RequireClaim("passport_type", "eur");
    });
    
});

var app = builder.Build();



app.UseAuthentication();

app.UseAuthorization();



//app.Use((ctx, next) =>
//{

//    if (ctx.Request.Path.StartsWithSegments("/login"))
//    {
//        return next();
//    }
//    //if(ctx.Request.Path.StartsWithSegments("/favicon.ico"))
//    //{
//    //    ctx.Response.StatusCode = 204;
//    //    return next();

//    //}
//    if (!ctx.User.Identities.Any(x => x.AuthenticationType == authScheme))
//    {
//        ctx.Response.StatusCode = 401;
//        return Task.CompletedTask;
//    }


//    if (!ctx.User.HasClaim("passport_type", "eur"))
//    {
//        ctx.Response.StatusCode = 403;
//        return Task.CompletedTask;
//    }

//    return next();
//});



app.MapGet("/username", (HttpContext ctx) =>
{

  return ctx.User.FindFirst("usr").Value ?? "empty";
});


//auth endpoint

app.MapGet("/sweden", (HttpContext ctx) =>
{
    //if(!ctx.User.Identities.Any(x => x.AuthenticationType == authScheme))
    //{
    //    ctx.Response.StatusCode = 401;
    //    return "";
    //}

    //if(!ctx.User.HasClaim("passport_type","eur"))
    //{
    //    ctx.Response.StatusCode = 403;
    //    return "";
    //}

    return "allowed";

   
}).RequireAuthorization("eu passport"); ;








app.MapGet("/login",async (HttpContext ctx) =>
{

    var claims = new List<Claim>();
    claims.Add(new Claim("usr","anton"));
    claims.Add(new Claim("passport_type", "eur"));
    var identity = new ClaimsIdentity(claims,authScheme);
    var user = new ClaimsPrincipal(identity);
    await ctx.SignInAsync(authScheme,user);
    return "ok";
}).AllowAnonymous();

app.UseHttpsRedirection();  



app.Run();


public class MyRequirement : IAuthorizationRequirement { }


public class MyRequirementHandler : AuthorizationHandler<MyRequirement>

{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirement requirement)
    {
        //throw new NotImplementedException();
        //context.User
        //context.Succeed(new  MyRequirement());

        return Task.CompletedTask;
    }
}




