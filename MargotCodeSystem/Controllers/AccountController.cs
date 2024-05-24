using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Models.Accounts;
using MargotCodeSystem.Models.Identity;
using MargotCodeSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace MargotCodeSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MargotCodeSystemDbContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, MargotCodeSystemDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register()
        {
            if (!_roleManager.RoleExistsAsync(RoleUtils.RoleSuperAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(RoleUtils.RoleSuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleUtils.RoleAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleUtils.RoleAuthor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(RoleUtils.RoleUser)).GetAwaiter().GetResult();

            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.UserName = model.Username;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var res = _userManager.CreateAsync(user, model.Password).GetAwaiter().GetResult();

                if (res.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, RoleUtils.RoleUser).GetAwaiter().GetResult();

                    var userId = user.Id;

                    // Now you need to create a new DashboardModel and set its UserId property
                    var dashboard = new DashboardModel
                    {
                        // Set other properties as needed
                        UserId = userId,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsActive = true
                    };
                    // Add the dashboard to the context and save changes
                    _context.Tbl_Dashboard.Add(dashboard);
                    _context.SaveChanges();

                    ViewBag.SuccessMessage = "User has been registered";
                }
                else
                {
                    List<IdentityError> errorList = res.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));

                    ViewBag.ErrorMessage = errors;
                }

            }
            return View(model);
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Cannot create an instance of '{nameof(ApplicationUser)}'");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var uName = model.Username;

                if (IsEmail(model.Username))
                {
                    var uData = _context.Users.FirstOrDefault(x => x.Email == model.Username);
                    if (uData != null)
                    {
                        uName = uData.UserName;
                    }
                }

                var res = _signInManager.PasswordSignInAsync(uName!, model.Password, model.RememberMe, false).GetAwaiter().GetResult();
                if (res.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Resident");
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
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Dashboard", "Resident");
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
