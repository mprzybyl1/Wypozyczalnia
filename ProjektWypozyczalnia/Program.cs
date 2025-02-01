using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjektWypozyczalnia.Data;
using ProjektWypozyczalnia.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}").RequireAuthorization("AdminOnly"); // RequireAuthorization 


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization("AdminOnly");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string adminEmail = "Test@admin.pl";
    string adminPassword = "ZAQ!2wsx";

    string userEmail = "Test@user.pl";
    string userPassword = "zaq1@WSX";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var user = new ApplicationUser();
        user.UserName = adminEmail;
        user.Email = adminEmail;

        await userManager.CreateAsync(user, adminPassword);
        await userManager.AddToRoleAsync(user, "Admin");
    }
    if (await userManager.FindByEmailAsync(userEmail) == null)
    {
        var user = new ApplicationUser();
        user.UserName = userEmail;
        user.Email = userEmail;

        await userManager.CreateAsync(user, userPassword);
        await userManager.AddToRoleAsync(user, "User");
    }

    var users = await userManager.Users.ToListAsync();
    foreach (var newUser in users)
    {
        if (!await userManager.IsInRoleAsync(newUser, "Admin") && !await userManager.IsInRoleAsync(newUser, "User"))
        {
            await userManager.AddToRoleAsync(newUser, "User");
        }
    }

}

app.Run();
