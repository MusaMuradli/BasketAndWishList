using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastKart.Areas.AdminPanel.Controllers
{
    public class DashboardController : AdminController
    {
   
        public IActionResult Index()
        {
            return View();
        }
    }
}
