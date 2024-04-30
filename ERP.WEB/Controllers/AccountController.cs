using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ERP.SERVICE.IRepositories.Auth;
using ERP.ENTITY.ViewModels.Auth;
using ERP.ENTITY.Models.Auth;
using ERP.ENTITY.Models.HRM._03.Employee;
using ERP.SERVICE;
using ERP.UTILITY;
using Microsoft.AspNetCore.Mvc.Rendering;
using ERP.SERVICE.Repositories.Auth;

namespace ERP.WEB.Controllers
{
    // AccountController.cs
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Other action methods...
        public async Task<IActionResult> Login()
        {
            LoginViewModel model = new LoginViewModel();
            var comlist = await _userRepository.GetCompanies();

            model.CompanyList = comlist.Select(c => new SelectListItem { Value = c.comcod, Text = c.comnam });


            return View("Login", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            // Use custom validation method from the repository


            bool isValidUser = await _userRepository.ValidateUser(model.Username, model.Password);
            //bool isValidUser = true;

                if (isValidUser)
                {
                //// Retrieve user information if needed
                User user = await _userRepository.GetUserByUsername(model.Username);

                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim("userid", user.userid),
                    new Claim("empid", user.empid),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, user.userrole),
                    new Claim(ClaimTypes.GivenName, user.userfname),
                    new Claim("userimg", user.userimg),
                    new Claim("mailid", user.mailid),
                


            


                    // Add other claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                return RedirectToAction("Index", "Home");
                }
            else
            {
                // Authentication failed, add error message to ModelState
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            //}

            var comlist = await _userRepository.GetCompanies();

            model.CompanyList = comlist.Select(c => new SelectListItem { Value = c.comcod, Text = c.comnam });

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View(); // Return a view for access denied
        }
        // Other methods...
    }

}
