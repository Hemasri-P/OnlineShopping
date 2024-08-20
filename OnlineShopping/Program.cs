using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(c=>c.Filters.Add(new  CustomExceptionFilterAttribute()));

builder.Services.AddSession();
builder.Services.AddOutputCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    c=> { c.LoginPath = "/Security/Login"; c.AccessDeniedPath = "/Security/accessdeniedpage"; });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

//Default method calling in Browser
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Onlineshop}/{action=Home}/{id?}");

app.Run();
