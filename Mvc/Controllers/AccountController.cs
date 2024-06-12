using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using System.Security.Claims;

namespace Mvc.Controllers;
public class AccountController() : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            // Very "secure". Don't try this at home.
            if (model.UserName == model.Password)
            {
                List<Claim> claims =
                    [
                        new("name", model.UserName),
                        new("email", model.UserName + "@example.com")
                    ];

                if (model.UserName == "Admin")
                {
                    claims.Add(new("role", "admin"));
                }

                var identity = new ClaimsIdentity(claims, "pwd", "name", "role");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError(nameof(model.Password), "Invalid User Name or Password");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignOut(string scheme)
    {
        if (scheme == CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await HttpContext.SignOutAsync(scheme);
            return Redirect("/");
        }

        AuthenticationProperties props = new()
        {
            RedirectUri = "/"
        };

        return SignOut(props, scheme);
    }

    public IActionResult Challenge(string scheme, string returnUrl)
    {
        returnUrl ??= "/";

        if (!Url.IsLocalUrl(returnUrl))
        {
            throw new Exception("Hackerzzzz");
        }

        AuthenticationProperties props = new()
        {
            RedirectUri = returnUrl,
        };

        return Challenge(props, scheme);
    }

    public IActionResult Forbid(string scheme)
    {
        return base.Forbid(scheme);
    }

    public IActionResult AccessDenied(string returnUrl)
    {
        return View(model: returnUrl);
    }

    public async Task<IActionResult> Authenticate(string scheme)
    {
        var authResult = await HttpContext.AuthenticateAsync(scheme);

        return View(authResult);
    }

    public async Task<IActionResult> Logout()
    {
        var authResult = await HttpContext.AuthenticateAsync();

        if (authResult.Succeeded)
        {
            if(authResult.Properties.Items.TryGetValue(".AuthScheme", out var schemeName))
            {
                AuthenticationProperties props = new()
                {
                    RedirectUri = "/"
                };

                return SignOut(props,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    schemeName!);
            }
        }

        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}
