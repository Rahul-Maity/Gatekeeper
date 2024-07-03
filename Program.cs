using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication()
    .AddScheme<CookieAuthenticationOptions,visitorAuthHandler>("visitor", o => { })
    .AddCookie("local")
    .AddCookie("patreon-cookie")
    .AddOAuth("external-patreon", o =>
    {
        o.SignInScheme = "patreon-cookie";
        o.ClientId = "id";
        o.ClientSecret = "secret";
        o.AuthorizationEndpoint = "https://oauth.wiremockapi.cloud/oauth/authorize";
        o.TokenEndpoint = "https://oauth.wiremockapi.cloud/oauth/token";
        o.UserInformationEndpoint = "https://oauth.wiremockapi.cloud/userinfo";
        o.CallbackPath = "/cb-patreon";
        o.Scope.Add("profile");
        o.SaveTokens = true;
    });

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("customer", p =>
    {
        p.AddAuthenticationSchemes("visitor", "local")
        .RequireAuthenticatedUser();
    });
    b.AddPolicy("user", p =>
    {
        p.AddAuthenticationSchemes("local")
        .RequireAuthenticatedUser();
    });
});

builder.Services.AddControllers();



var app = builder.Build();



app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/", ctx => Task.FromResult("hello world")).RequireAuthorization("customer");

app.MapGet("/login-local", async (ctx) =>
{

    var claims = new List<Claim>();
    claims.Add(new Claim("user", "anton"));
    var identity = new ClaimsIdentity(claims, "local");

    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("local", user);
    

});

app.MapGet("/login-patreon", async (ctx) =>
{

    await ctx.ChallengeAsync("external-patreon");


}).RequireAuthorization("user");



app.UseHttpsRedirection();
app.MapControllers();



app.Run();





public class visitorAuthHandler : CookieAuthenticationHandler
{
    public visitorAuthHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {

    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await base.HandleAuthenticateAsync();
        if(result.Succeeded)
        {
            return result;
        }
        var claims = new List<Claim>();
        claims.Add(new Claim("user", "anton"));
        var identity = new ClaimsIdentity(claims, "visitor");

        var user = new ClaimsPrincipal(identity);

        Context.SignInAsync("visitor", user);
        return AuthenticateResult.Success(new AuthenticationTicket(user,"visitor"));


    }
}