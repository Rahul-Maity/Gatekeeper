namespace Gatekeeper;

public static class BuildExtension
{
    public static WebApplication BuildWithSpa(this WebApplicationBuilder webApplicationBuilder)
    {
        var app = webApplicationBuilder.Build();


        app.UseRouting();

        app.UseAuthentication();


        app.UseEndpoints(_ => { });


        app.Use( (context, next) =>
        {
            if(context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            }
            return next();

        } );

        app.UseSpa(x => { x.UseProxyToSpaDevelopmentServer("http://127.0.0.1:4200"); });
        return app;
    }

}
