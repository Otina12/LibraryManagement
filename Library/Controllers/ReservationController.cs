using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class ReservationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
