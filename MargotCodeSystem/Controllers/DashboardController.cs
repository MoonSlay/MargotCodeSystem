using Microsoft.AspNetCore.Mvc;

namespace MargotCodeSystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
