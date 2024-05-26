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
using MargotCodeSystem.Utils;

namespace MargotCodeSystem.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    public class ResidentController(MargotCodeSystemDbContext context) : Controller
    {
        private readonly MargotCodeSystemDbContext _context = context;

        [HttpGet]
        public IActionResult Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dashboardList = _context.Tbl_Dashboard
                .Where(d => d.UserId == userId && d.IsActive == true)
                .Select(d => new DashboardModel
                {
                    Id = d.Id,
                    fullName = EncryptionHelper.DecryptString(d.fullName),
                    provincialAddress = EncryptionHelper.DecryptString(d.provincialAddress),
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

                model.Fullname = EncryptionHelper.EncryptString($"{model.LastName}, {model.FirstName} {model.MiddleName}.");
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                model.IsActive = true;
                model.UserId = user?.Id;

                _context.Tbl_Residents.Add(model);
                _context.SaveChanges();

                foreach (var pet in model.Pets)
                {
                    pet.DateCreated = DateTime.Now;
                    pet.DateModified = DateTime.Now;
                    pet.IsActive = false;
                    pet.ResidentId = model.Id;
                    _context.Tbl_Pets.Add(pet);
                }

                foreach (var med in model.Meds)
                {
                    med.DateCreated = DateTime.Now;
                    med.DateModified = DateTime.Now;
                    med.IsActive = false;
                    med.ResidentId = model.Id;
                    _context.Tbl_Meds.Add(med);
                }

                if (user != null)
                {
                    var dashboardModel = new DashboardModel
                    {
                        fullName = EncryptionHelper.DecryptString(model.Fullname),
                        provincialAddress = model.ProvincialAddress,
                        seniorCitizen = model.SeniorCitizen,
                        medicationUser = model.TakingMeds,
                        streetSweeper = model.StreetSweeper,
                        petOwner = model.PetOwner,
                        activeResident = model.ActiveResident,
                        ResidentId = model.Id,
                        UserId = user.Id,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsActive = true
                    };
                    _context.Tbl_Dashboard.Add(dashboardModel);
                    _context.SaveChanges();
                }
                else
                {
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
                        FullName = EncryptionHelper.DecryptString(o.FullName),
                        Position = EncryptionHelper.DecryptString(o.Position),
                        Age = EncryptionHelper.DecryptString(o.Age),
                        BirthDate = EncryptionHelper.DecryptString(o.BirthDate),
                        CivilStatus = EncryptionHelper.DecryptString(o.CivilStatus),
                        SourceIncome = EncryptionHelper.DecryptString(o.SourceIncome)
                    }).ToList()
                })
                .ToList();

            resident.Fullname = EncryptionHelper.DecryptString(resident.Fullname);

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
            var resident = _context.Tbl_Residents.Find(id);
            if (resident == null)
            {
                return NotFound();
            }

            var viewModel = new ResidentViewModel
            {
                Resident = resident,
            };

            return View("PrintResidentDetails", viewModel);
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