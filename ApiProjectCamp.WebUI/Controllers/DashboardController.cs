using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
