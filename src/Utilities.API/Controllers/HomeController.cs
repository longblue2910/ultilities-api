using Microsoft.AspNetCore.Mvc;

namespace Utilities.API.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
