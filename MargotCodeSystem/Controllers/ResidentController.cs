using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Models.Accounts;
using MargotCodeSystem.Models.Identity;
using MargotCodeSystem.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MargotCodeSystem.Controllers
{
    public class ResidentController : Controller
    {
        private readonly MargotCodeSystemDbContext _context;

        public ResidentController(MargotCodeSystemDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var dashboardList = _context.Tbl_Dashboard
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

        [HttpGet]
        public IActionResult Occupant()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddResident()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResident(ResidentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Fullname = model.LastName + ", " + model.FirstName + " " + model.MiddleName + ".";
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                model.IsActive = true;

                _context.Tbl_Residents.Add(model);
                _context.SaveChanges();

                var dashboardModel = new DashboardModel
                {
                    fullName = model.Fullname,
                    provincialAddress = model.ProvincialAddress,
                    seniorCitizen = model.SeniorCitizen,
                    medicationUser = model.TakingMeds,
                    streetSweeper = model.StreetSweeper,
                    petOwner = model.PetOwner,
                    activeResident = model.ActiveResident,
                    ResidentId = model.Id, // Set the foreign key ResidentId with the newly generated ResidentId
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true
                };
                _context.Tbl_Dashboard.Add(dashboardModel);
                _context.SaveChanges();

                var occupant = new HouseOccupantModel
                {
                    fullName = model.Fullname,
                    BirthDate = model.DateOfBirth,
                    CivilStatus = model.CivilStatus,
                    ResidentId = model.Id,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsActive = true

                };
                _context.Tbl_HouseOccupants.Add(occupant);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Resident");
            }
            return View(model);
        }
    }
}