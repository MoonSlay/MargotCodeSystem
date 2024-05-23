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
            var houseOccupantGroups = new List<HouseOccupantGroupModel>();

            var occupantGroups = _context.Tbl_HouseOccupants
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

            return View(occupantGroups);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHouse(HouseOccupantModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Set creation and modification dates, and mark as active
                    model.DateCreated = DateTime.Now;
                    model.DateModified = DateTime.Now;
                    model.IsActive = true;

                    // Add the new house occupant to the database
                    _context.Tbl_HouseOccupants.Add(model);
                    _context.SaveChanges();

                    // Redirect to the HouseDashboard after successful save
                    return RedirectToAction("HouseDashboard", "House");
                }
                else
                {
                    // If model state is not valid, return the view with validation errors
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Log the exception to the console or any other logging mechanism
                Console.WriteLine(ex.Message);

                // Set an error message to be displayed in the view
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";

                // Return the view with the error message
                return View(model);
            }

        }
    }
}
