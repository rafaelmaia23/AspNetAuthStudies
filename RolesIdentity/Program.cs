using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(c => c.UseInMemoryDatabase("Identity"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>( o => 
{
    o.User.RequireUniqueEmail = false;

    o.Password.RequireDigit = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await roleMgr.CreateAsync(new IdentityRole() { Name = "admin" });
    var usrMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var user = new IdentityUser() { UserName = "test@test.com", Email = "test@test.com" };
    await usrMgr.CreateAsync(user, password: "password");
    await usrMgr.AddToRoleAsync(user, "admin");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
