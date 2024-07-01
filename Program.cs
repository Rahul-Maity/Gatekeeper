using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthentication()
    .AddCookie("cookie");

builder.Services.AddAuthorization();

builder.Services.AddControllers();



var app = builder.Build();



app.UseAuthentication();

app.UseAuthorization();



app.UseHttpsRedirection();
app.MapControllers();



app.Run();





