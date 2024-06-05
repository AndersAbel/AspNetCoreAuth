using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mvc.Models;

public class LoginModel
{
    [Display(Name = "User Name")]
    public string UserName { get; set; } = default!;

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [FromQuery]
    public string? ReturnUrl { get; set; }
}
