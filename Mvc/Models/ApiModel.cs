namespace Mvc.Models;

public class ApiModel
{
    public int StatusCode { get; set; }

    public string? Content { get; set; }

    public string Url { get; set; } = default!;
}
