using Microsoft.AspNetCore.Mvc;

namespace Bilet1.Controllers;

public class BlogController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
