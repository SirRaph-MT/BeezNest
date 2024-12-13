
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Logic.Helper;
using Logic.IHelper;
using Core.Db;
using Core.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAdminHelper, AdminHelper>();

builder.Services.AddScoped<IUserHelper, UserHelper>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BeezNest"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
UpdateDatabase(app);
app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
static void UpdateDatabase(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope())
    {
        using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
        {
            context?.Database.Migrate();
        }
    }
}


//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Logic.Helper;
//using Logic.IHelper;
//using Core.Db;
//using Core.Models;
//using System;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// Enable session storage for managing user sessions
//builder.Services.AddSession();

//// Enable access to the current HTTP context (required for user-related services)
//builder.Services.AddHttpContextAccessor();

//// Register AdminHelper as a scoped service for dependency injection
//builder.Services.AddScoped<IAdminHelper, AdminHelper>();

//// Register UserHelper as a scoped service for dependency injection
//builder.Services.AddScoped<IUserHelper, UserHelper>();

//// Register your database context with Entity Framework and use the connection string from appsettings.json
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("BeezNest"));
//});

//// Configure Identity services
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = false; // Disable requirement for numeric characters in passwords
//    options.Password.RequiredLength = 5; // Set minimum password length
//    options.Password.RequiredUniqueChars = 0; // Allow passwords without unique characters
//    options.Password.RequireLowercase = false; // Disable requirement for lowercase characters in passwords
//    options.Password.RequireNonAlphanumeric = false; // Disable requirement for special characters in passwords
//    options.Password.RequireUppercase = false; // Disable requirement for uppercase characters in passwords
//})
//.AddEntityFrameworkStores<ApplicationDbContext>() // Specify the database context to use for Identity
//.AddDefaultTokenProviders(); // Add token providers for functionalities like email verification or password reset

//// Add a custom claims principal factory (needed for adding custom claims like "FirstName")
//builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error"); // Redirect to error page in production
//    app.UseHsts(); // Enforce HTTP Strict Transport Security (HSTS) for secure communication
//}

//app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
//app.UseStaticFiles(); // Serve static files (CSS, JS, images, etc.)

//UpdateDatabase(app); // Apply migrations at runtime

//app.UseRouting(); // Enable routing middleware

//// Enable session middleware (needed for maintaining state across requests)
//app.UseSession();

//app.UseAuthentication(); // Enable authentication middleware (required for Identity)
//app.UseAuthorization(); // Enable authorization middleware

//// Map default controller route
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

//// Helper method to apply pending migrations at runtime
//static void UpdateDatabase(IApplicationBuilder app)
//{
//    using (var serviceScope = app.ApplicationServices
//        .GetRequiredService<IServiceScopeFactory>()
//        .CreateScope())
//    {
//        using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
//        {
//            context?.Database.Migrate();
//        }
//    }
//}
