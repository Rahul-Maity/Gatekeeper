

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddAuthentication()
    .AddCookie("Default", o =>
    {
        o.Cookie.Name = "mycookie";
        //o.Cookie.Path = "/test"; 
        o.Cookie.HttpOnly = false;
    });

var app = builder.Build();
app.UseAuthentication();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.MapGet("/", () => "Hello world");

app.MapPost("/login",  async (HttpContext ctx) =>
{


    await ctx.SignInAsync("Default", new ClaimsPrincipal(
               new ClaimsIdentity(
                   new Claim[]
                   {
                        new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
                   }, "Default"

                   )


               ));
    return "ok";


});
app.Run();
