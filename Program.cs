using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(c => c.UseInMemoryDatabase("my_db"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
{
    o.User.RequireUniqueEmail = true;
    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddCookie("cookie");

builder.Services.AddAuthorization();

builder.Services.AddControllers();



var app = builder.Build();



app.UseAuthentication();

app.UseAuthorization();



using (var scope = app.Services.CreateScope())
{
    var usrMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await roleMgr.CreateAsync(new IdentityRole() {Name = "admin" });
    
    var user = new IdentityUser() { UserName = "test@test.com", Email = "test@test.com" };
    await usrMgr.CreateAsync(user,password:"password");
   await  usrMgr.AddToRoleAsync(user, "admin");
}


app.UseHttpsRedirection();
app.MapControllers();



app.Run();





