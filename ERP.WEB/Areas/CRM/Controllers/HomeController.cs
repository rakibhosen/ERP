using Microsoft.AspNetCore.Mvc;

namespace ERP.WEB.Areas.CRM.Controllers
{
    [Area("CRM")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
