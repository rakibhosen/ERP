using System.Threading.Tasks;
using ERP.ENTITY.Models.HRM._01.CodeBook;
using ERP.SERVICE.IRepositories.HRM;
using ERP.SERVICE.Repositories.HRM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace ERP.WEB.Areas.HRM.Controllers
{
    [Area("HRM")]
    [Route("HRM/[Controller]/[Action]")]
    public class CodeBookController : Controller
    {
        private readonly ICodeBookReposiroty _codeBookRepository;

        public CodeBookController(ICodeBookReposiroty codeBookRepository)
        {
            _codeBookRepository = codeBookRepository;
        }

        // For the Index method without parameters
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var codeBookList = await _codeBookRepository.GetCode();
            if (codeBookList == null)
            {
                return NotFound();
            }

            ViewBag.CodeBookList = codeBookList;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string id)
        {
            var codeBookList = await _codeBookRepository.GetCode();
            if (codeBookList == null)
            {
                return NotFound();
            }

            ViewBag.CodeBookList = codeBookList;
            if (id != null)
            {
                return View(codeBookList);
            }
            else
            {
                return View();
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddSubCode(string codeId, [Bind("comcod, hrgcod, hrgdesc, hrgval")] codebook codeBook)
        {
            if (ModelState.IsValid)
            {
                var result = await _codeBookRepository.AddCode(codeBook);
                if (result != null)
                {
                    // Handle successful addition
                    var codeBookList = await _codeBookRepository.GetCode(); // Retrieve updated list of subcodes
                    ViewBag.CodeBookList = codeBookList; // Set the updated list in ViewBag
                    return View("Index", codeBookList); // Render the Index view with the updated list
                }
            }
            // Handle validation errors or failed addition
            return RedirectToAction("Index", "CodeBook", new { id = codeId }); // Redirect to Index view if addition fails
        }



    }
}
