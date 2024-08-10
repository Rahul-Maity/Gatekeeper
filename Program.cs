

using System.Security.Claims;

using Gatekeeper;
using Gatekeeper.Handler;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");


var app = builder.BuildWithSpa();

#region EndPoints setup

var apiEndpoints = app.MapGroup("/api");

apiEndpoints.MapGet("/", () => "hello world");


apiEndpoints.MapGet("/user", (ClaimsPrincipal user) =>
{
    user.Claims.ToDictionary(x => x.Type, x => x.Value);
});


apiEndpoints.MapGet("/login",(LoginForm form)=>LoginHandler.HandleLogin(form) );

apiEndpoints.MapGet("register", () => "todo");

apiEndpoints.MapGet("logout", () => Results.SignOut(authenticationSchemes: new List<string>() { "cookie" }));



#endregion

app.Run();



public class LoginForm
{
    public string Username { get; set; }
    public string Password { get; set; }
}