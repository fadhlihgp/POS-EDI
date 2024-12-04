using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml;
using POS_Frontend.Services;
using POS_Frontend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject HttpConfiguration
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("PosApp", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["IntegrationApi:BackendUrl"]);
    // client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddNotyf(config=>
    {
        config.DurationInSeconds = 7;
        config.IsDismissable = true;
        config.Position = NotyfPosition.TopRight; }
);

builder.Services.AddTransient<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<IBaseService, BaseService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IPurchaseService, PurchaseService>();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        // options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // Durasi sesi login
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();