using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.SERVICE.IRepositories.HRM;
using ERP.UTILITY;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ERP.WEB.Areas.HRM.Controllers
{
    [Authorize]
    [Area("HRM")]
    [Route("HRM/[Controller]/[Action]")]
    public class HomeController : Controller
    {


        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetEmployees();
       
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employee updatedEmployee, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
               
                string fileLocation = "~/Images/";
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Specify the path where the file will be saved
                var path = Path.Combine(_env.WebRootPath, "Images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                updatedEmployee.EmpImg = fileLocation+fileName;
            }

            bool result = await _employeeRepository.UpdateEmployee(updatedEmployee);

            return RedirectToAction("Index");
        }




    }
}
