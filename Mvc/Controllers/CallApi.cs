using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Mvc.Models;

namespace Mvc.Controllers;
public class CallApi : Controller
{
    HttpClient httpClient = new();

    public async Task<IActionResult> Index(string method)
    {
        if (User.Identity!.IsAuthenticated)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new("Bearer", token);
            }
        }
        var url = $"https://localhost:5005/{method}";

        var result = await httpClient.GetAsync(url);

        var model = new ApiModel
        {
            StatusCode = (int)result.StatusCode,
            Url = url
        };

        if (result.IsSuccessStatusCode)
        {
            model.Content = await result.Content.ReadAsStringAsync();
        }

        return View(model);
    }
}
