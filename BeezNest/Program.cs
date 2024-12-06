using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Logic.Helper;
using Logic.IHelper;
using Core.Db;
using Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register session handling and IHttpContextAccessor for accessing HttpContext in helpers
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Register custom helpers
builder.Services.AddScoped<IAdminHelper, AdminHelper>();
//builder.Services.AddScoped<ICartHelper, CartHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();

// Register DbContext and Identity
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

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
