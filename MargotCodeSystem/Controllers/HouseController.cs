using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MargotCodeSystem.Controllers
{
    public class HouseController(MargotCodeSystemDbContext context) : Controller
    {
        private readonly MargotCodeSystemDbContext _context = context;

        //Get House Occupant View
        [HttpGet]
        public IActionResult HouseDashboard()
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
        public IActionResult AddHouse()
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
        public IActionResult AddHouse(HouseOccupantModel model)
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
