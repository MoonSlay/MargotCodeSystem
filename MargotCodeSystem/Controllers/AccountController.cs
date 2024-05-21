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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationModel model)
        {
            try
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

                        // Retrieve the ID of the created user
                        var userId = user.Id;

                        // Now you need to create a new DashboardModel and set its UserId property
                        var dashboard = new DashboardModel
                        {
                            // Set other properties as needed
                            UserId = userId,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true // Assuming you want the dashboard to be active by default
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                // Log the exception for further investigation
                Console.WriteLine(ex.Message);
                return View(model);
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
