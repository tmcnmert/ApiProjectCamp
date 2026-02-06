using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebUI.ViewComponents.DashboardViewComponents
{
    public class _DashboardClaudeAIComponentPartial:ViewComponent
    {
          
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
