using Microsoft.EntityFrameworkCore;
using NAPA.Database;
using NAPA.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ApplicationContext>(_ => new ApplicationContext(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationContext>();

builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddAuthentication().AddCookie(options =>
{
    options.LoginPath = "Account/login";
    options.LogoutPath = "Account/logout";
    options.AccessDeniedPath = "Account/AccessDenied";
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
