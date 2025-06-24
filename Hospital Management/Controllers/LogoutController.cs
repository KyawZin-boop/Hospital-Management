using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}
