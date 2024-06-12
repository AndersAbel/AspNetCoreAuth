using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddJwtBearer(opt =>
    {
        opt.Authority = "https://demo.duendesoftware.com";
        opt.TokenValidationParameters.ValidateAudience = false;

        opt.MapInboundClaims = false;
    });

var app = builder.Build();

app.MapGet("/challenge", async ctx =>
{
    await ctx.ChallengeAsync();
});

app.MapGet("/authenticate", async ctx =>
{
    var result = await ctx.AuthenticateAsync();

    if (result.Succeeded)
    {
        await ctx.Response.WriteAsJsonAsync(
            result.Principal.Claims.Select(c => new { c.Type, c.Value }));
    }
    else
    {
        await ctx.Response.WriteAsync("Authenticate failed");
    }
});

app.MapGet("/forbid", async ctx =>
{
    await ctx.ForbidAsync();
});

app.Run();