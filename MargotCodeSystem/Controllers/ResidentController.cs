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

                foreach (var pet in model.Pets)
                {
                    // Set other properties of the pet if needed
                    pet.DateCreated = DateTime.Now;
                    pet.DateModified = DateTime.Now;
                    pet.IsActive = false;
                    // Assign foreign key
                    pet.ResidentId = model.Id;
                    _context.Tbl_Pets.Add(pet);
                }

                foreach (var med in model.Meds)
                {
                    // Set other properties of the med if needed
                    med.DateCreated = DateTime.Now;
                    med.DateModified = DateTime.Now;
                    med.IsActive = false;
                    // Assign foreign key
                    med.ResidentId = model.Id;
                    _context.Tbl_Meds.Add(med);
                }

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var occupantGroups = _context.Tbl_HouseOccupants
                .Where(o => o.UserId == userId) // Filter by UserId
                .Where(o => _context.Tbl_HouseGroup.Any(h => h.ResidentId == id && h.HouseName == o.HouseName))
                .GroupBy(o => o.HouseName)
                .Select(g => new HouseOccupantGroupModel
                {
                    HouseName = g.Key,
                    HouseOccupants = g.Select(o => new HouseOccupantModel
                    {
                        Id = o.Id,
                        fullName = o.fullName,
                        Position = o.Position,
                        Age = o.Age,
                        BirthDate = o.BirthDate,
                        CivilStatus = o.CivilStatus,
                        SourceIncome = o.SourceIncome
                    }).ToList()
                })
                .ToList();

            var viewModel = new ResidentViewModel
            {
                Resident = resident,
                HouseoccupantGroup = occupantGroups
            };
            return View(viewModel);
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