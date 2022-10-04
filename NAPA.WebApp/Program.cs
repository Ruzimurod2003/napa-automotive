using Microsoft.EntityFrameworkCore;
using NAPA.Database;
using NAPA.Models;

string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
string databaseName = "NAPA.db";
string connectionString = "Data Source=" + Path.Combine(projectPath, databaseName);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationContext>(_ => new ApplicationContext(connectionString));

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
