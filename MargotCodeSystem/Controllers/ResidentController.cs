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

        // GET: Resident/Create
        public IActionResult Index()
        {
            return View();
        }

        // POST: Resident/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ResidentModel model)
        {
            if (ModelState.IsValid)
            {
                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;

                _context.Tbl_Residents.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        private ResidentModel CreateResident()
        {
            try
            {
                return Activator.CreateInstance<ResidentModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Cannot create an instance of '{nameof(ResidentModel)}'");
            }
        }
    }
}