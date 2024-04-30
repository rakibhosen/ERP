using Microsoft.AspNetCore.Mvc;

namespace ERP.WEB.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult Index()
        {
            // Load modules from the database
            //var modules = _dataService.GetModules();
            var modules = "";

            // Pass modules to the partial view
            ViewData["Modules"] = modules;

            return View();
        }
    }
}
