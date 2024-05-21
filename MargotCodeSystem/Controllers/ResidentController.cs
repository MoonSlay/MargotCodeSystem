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


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ResidentModel model)
        {
            if (ModelState.IsValid)
            {
                model.FirstName = model.LastName + ", " + model.FirstName + " " + model.MiddleName + ".";
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                model.IsActive = true;

                _context.Tbl_Residents.Add(model);
                _context.SaveChanges();

                //var dashboardModel = new DashboardModel
                //{
                //    fullName = model.FullName,
                //    seniorCitizen = model.SeniorCitizen,
                //    medicationUser = model.TakingMeds,
                //    streetSweeper = model.StreetSweeper,
                //    petOwner = model.PetOwner,
                //    activeResident = model.ActiveResident,
                //    ResidentId = model.Id, // Set the foreign key ResidentId with the newly generated ResidentId
                //    DateCreated = DateTime.Now,
                //    DateModified = DateTime.Now,
                //    IsActive = true
                //};

                //_context.Tbl_Dashboard.Add(dashboardModel);
                //_context.SaveChanges();

                return RedirectToAction("Index", "Resident");
            }
            return View(model);
        }
    }
}