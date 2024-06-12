using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDataProtection()
    .SetApplicationName("NDC")
    .PersistKeysToFileSystem(new("c:\\temp\\dpkeys"))
    .ProtectKeysWithCertificate(Helpers.Thumbprint);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddOpenIdConnect(opt =>
    {
        // There's some configuration parameters needed to connect to
        // Duende Software's demo IdentityServer instance. That's not
        // the main focus of the talk where I use this demo, so I've
        // hidden those in a separate method.
        opt.ConfigureIdentityServer();
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("TopSecret", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireAssertion(ctx =>
            ctx.User.FindFirstValue("name")!.StartsWith("A"));
    });
});

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{scheme=" + CookieAuthenticationDefaults.AuthenticationScheme + "}");

app.Run();
