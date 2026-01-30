using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult SendChatWithAI()
        {
            return View();
        }
    }
}
