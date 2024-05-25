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
                .Where(d => d.UserId == userId && d.IsActive == true)
                .Select(d => new DashboardModel
                {
                    Id = d.Id,
                    fullName = d.fullName,
                    provincialAddress = d.provincialAddress,
                    seniorCitizen = d.seniorCitizen == true, // Convert bit to bool
                    medicationUser = d.medicationUser == true, // Convert bit to bool
                    streetSweeper = d.streetSweeper == true, // Convert bit to bool
                    petOwner = d.petOwner == true, // Convert bit to bool
                    activeResident = d.activeResident == true, // Convert bit to bool
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
                        seniorCitizen = model.SeniorCitizen == true, // Convert bit to bool
                        medicationUser = model.TakingMeds == true, // Convert bit to bool
                        streetSweeper = model.StreetSweeper == true, // Convert bit to bool
                        petOwner = model.PetOwner == true, // Convert bit to bool
                        activeResident = model.ActiveResident == true, // Convert bit to bool
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

        [HttpGet]
        public IActionResult PrintDetails(int id)
        {
            // Fetch the resident details from the database
            var resident = _context.Tbl_Residents.Find(id);
            if (resident == null)
            {
                return NotFound();
            }

            return View("PrintResidentDetails", resident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteResident(int id)
        {
            var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            // Soft delete: Set IsActive to false instead of removing from database
            resident.IsActive = false;
            _context.SaveChanges();

            // Update corresponding dashboard entry
            var dashboardEntry = _context.Tbl_Dashboard.FirstOrDefault(d => d.ResidentId == id);
            if (dashboardEntry != null)
            {
                dashboardEntry.IsActive = false;
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Resident");
        }
    }
}