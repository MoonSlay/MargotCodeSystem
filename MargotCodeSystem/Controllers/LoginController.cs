using MargotCodeSystem.Database;
using MargotCodeSystem.Models.Accounts;
using MargotCodeSystem.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace CodeFirstDatabase.Controllers
{
    [AllowAnonymous]
    public class LoginController(SignInManager<ApplicationUser> signManager, MargotCodeSystemDbContext margotCodeSystemDbContext) : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signManager;
        private readonly MargotCodeSystemDbContext _margotCodeSystemDbContext = margotCodeSystemDbContext;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var uName = model.Username;

                if (IsEmail(model.Username))
                {
                    var uData = _margotCodeSystemDbContext.Users.FirstOrDefault(x => x.Email == model.Username);
                    if (uData != null)
                    {
                        uName = uData.UserName;
                    }
                }

                var res = _signInManager.PasswordSignInAsync(uName!, model.Password, model.RememberMe, false).GetAwaiter().GetResult();
                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username/password";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                _signInManager.SignOutAsync().GetAwaiter().GetResult();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }

        }

        public bool IsEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
