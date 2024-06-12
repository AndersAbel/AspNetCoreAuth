using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

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