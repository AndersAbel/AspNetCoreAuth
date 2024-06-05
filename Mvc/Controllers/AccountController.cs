using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using System.Security.Claims;

namespace Mvc.Controllers;
public class AccountController : Controller
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

                if(model.UserName == "Admin")
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
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}
