using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MargotCodeSystem.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    public class ResidentController(MargotCodeSystemDbContext context) : Controller
    {
        private readonly MargotCodeSystemDbContext _context = context;

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Retrieve the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the dashboard entries associated with the current user
            var dashboardList = _context.Tbl_Dashboard
                .Where(d => d.UserId == userId)
                .Select(d => new DashboardModel
                {
                    Id = d.Id,
                    fullName = d.fullName,
                    provincialAddress = d.provincialAddress,
                    seniorCitizen = d.seniorCitizen,
                    medicationUser = d.medicationUser,
                    streetSweeper = d.streetSweeper,
                    petOwner = d.petOwner,
                    activeResident = d.activeResident,
                })
                .ToList();

            return View(dashboardList);
        }

        //Get Adding Resident View
        [HttpGet]
        public IActionResult AddResident()
        {
            return View();
        }


        //Adding Resident Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResident(ResidentModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Tbl_ApplicationUsers
                .FirstOrDefault(u => u.UserName == User.Identity.Name);

                model.Fullname = model.LastName + ", " + model.FirstName + " " + model.MiddleName + ".";
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                model.IsActive = true;
                model.UserId = user?.Id;

                _context.Tbl_Residents.Add(model);
                _context.SaveChanges();

                if (user != null)
                {
                    var dashboardModel = new DashboardModel
                    {
                        fullName = model.Fullname,
                        provincialAddress = model.ProvincialAddress,
                        seniorCitizen = model.SeniorCitizen,
                        medicationUser = model.TakingMeds,
                        streetSweeper = model.StreetSweeper,
                        petOwner = model.PetOwner,
                        activeResident = model.ActiveResident,
                        ResidentId = model.Id,
                        UserId = user.Id, // Set UserId as the Id from the ApplicationUser
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsActive = true
                    };
                    _context.Tbl_Dashboard.Add(dashboardModel);
                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case where the user is not found
                    return RedirectToAction("Error", "Home");
                }

                return RedirectToAction("Dashboard", "Resident");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewResident(int id)
        {
            var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            return View(resident);
        }
    }
}