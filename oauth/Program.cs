var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication()
    .AddOAuth("github", options =>
    {
        options.ClientId = "";
        options.ClientSecret = "";
        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";

        options.UserInformationEndpoint = "https://api.github.com/user";
        options.CallbackPath = "/https://127.0.0.1:5005";
    });


var app = builder.Build();


app.MapGet("/", (HttpContext ctx) =>
{
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});


app.MapGet("/login", () =>
{
    return Results.Challenge(authenticationSchemes: new List<string>() { "github"});
});


app.Run();

