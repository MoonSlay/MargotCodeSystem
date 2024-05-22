using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Models.Accounts;
using MargotCodeSystem.Models.Identity;
using MargotCodeSystem.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //Get Resident Dashboard View
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

                return RedirectToAction("Dashboard", "Resident");
            }
            return View(model);
        }

        //Get House Occupant View
        [HttpGet]
        public IActionResult OccupantDb()
        {
            var occupantList = _context.Tbl_HouseOccupants
                .Select(d => new HouseOccupantModel
                {
                    Id = d.Id,
                    fullName = d.fullName,
                    Position = d.Position,
                    Age = d.Age,
                    BirthDate = d.BirthDate,
                    CivilStatus = d.CivilStatus,
                    SourceIncome = d.SourceIncome,
                })
                .ToList();

            return View(occupantList);
        }

        //Get Resident Details For House Occupant
        [HttpGet]
        public JsonResult GetResidentDetails(int id)
        {
            var resident = _context.Tbl_Residents
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Fullname,
                    r.DateOfBirth,
                    r.CivilStatus
                })
                .FirstOrDefault();

            return Json(resident);
        }

        //Get Adding House View
        [HttpGet]
        public IActionResult AddHouseOccupant()
        {
            var residents = _context.Tbl_Residents
                .Select(r => new { r.Id, r.Fullname })
                .ToList();

            ViewBag.Residents = new SelectList(residents, "Id", "Fullname");
            return View();
        }

        //Adding House Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHouseOccupant(HouseOccupantModel model)
        {
            if (ModelState.IsValid)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                model.IsActive = true;

                var selectedResident = _context.Tbl_Residents.Find(model.ResidentId);
                if (selectedResident != null)
                {
                    model.fullName = selectedResident.Fullname;
                    model.BirthDate = selectedResident.DateOfBirth;
                    model.CivilStatus = selectedResident.CivilStatus;
                }

                _context.Tbl_HouseOccupants.Add(model);
                _context.SaveChanges();
                return RedirectToAction("OccupantDb", "Resident");
            }
            var residents = _context.Tbl_Residents
                .Select(r => new { r.Id, r.Fullname })
                .ToList();
            ViewBag.Residents = new SelectList(residents, "Id", "Fullname");
            return View(model);
        }
    }
}