var builder = WebApplication.CreateBuilder(args);



var app = builder.Build();


app.MapGet("/username", (HttpContext ctx) =>
{
    if (!ctx.Request.Cookies.TryGetValue("auth", out var authCookie))
    {
        return "no auth cookie found";
    }

    var parts = authCookie.Split(':');
    var key = parts[0];
    var value = parts[1];
    return value;
});


app.MapGet("/login", (HttpContext ctx) =>
{
    ctx.Response.Headers["set-cookie"] = "auth=usr:anton";
    return "ok";
});

app.UseHttpsRedirection();



app.Run();

