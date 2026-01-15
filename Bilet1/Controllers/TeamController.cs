using Microsoft.AspNetCore.Mvc;

namespace Bilet1.Controllers;

public class TeamController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
