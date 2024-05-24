using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MargotCodeSystem.Controllers
{
    public class HouseController(MargotCodeSystemDbContext context) : Controller
    {
        private readonly MargotCodeSystemDbContext _context = context;

        //Get House Occupant View
        [HttpGet]
        public IActionResult HouseDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var houseOccupantGroups = new List<HouseOccupantGroupModel>();

            var occupantGroups = _context.Tbl_HouseOccupants
                .Where(o => o.UserId == userId) // Filter by UserId
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
            try
            {
                // Populate ViewBag.Residents with data for dropdown list
                ViewBag.Residents = _context.Tbl_Residents
                    .Select(r => new SelectListItem
                    {
                        Value = r.CivilStatus.ToString(),
                        Text = $"{r.Fullname}"
                    })
                    .ToList();

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddHouse(HouseOccupantModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Tbl_ApplicationUsers
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                    // Retrieve the selected resident
                    var resident = _context.Tbl_Residents.FirstOrDefault(r => r.CivilStatus == model.CivilStatus);
                    if (resident != null)
                    {
                        model.fullName = resident.Fullname;
                        model.BirthDate = resident.DateOfBirth;
                        // Set other required fields
                        model.DateCreated = DateTime.Now;
                        model.DateModified = DateTime.Now;
                        model.IsActive = true;
                        model.UserId = user.Id;

                        // Add HouseOccupantModel to database
                        _context.Tbl_HouseOccupants.Add(model);
                        _context.SaveChanges();

                        var House = new HouseOccupantGroupModel
                        {
                            HouseName = model.HouseName,
                            UserId = user.Id,
                        };
                        _context.Tbl_HouseGroup.Add(House);
                        _context.SaveChanges();

                        return RedirectToAction(nameof(HouseDashboard));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Selected resident not found.");
                    }
                }
                // Repopulate the dropdown list if there's an error
                ViewBag.Residents = _context.Tbl_Residents
                    .Select(r => new SelectListItem
                    {
                        Value = r.CivilStatus.ToString(),
                        Text = $"{r.Fullname}"
                    })
                    .ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                return View(model);
            }
        }

        //Get Adding House View
        [HttpGet]
        public IActionResult AddOccupant()
        {
            try
            {
                // Populate ViewBag.Residents with data for dropdown list
                ViewBag.Residents = _context.Tbl_Residents
                    .Select(r => new SelectListItem
                    {
                        Value = r.CivilStatus.ToString(),
                        Text = $"{r.Fullname}"
                    })
                    .ToList();

                ViewBag.House = _context.Tbl_HouseGroup
                   .Select(r => new SelectListItem
                   {
                       Value = r.HouseName,
                       Text = $"{r.HouseName}"
                   })
                   .ToList();

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOccupant(HouseOccupantModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Retrieve the selected resident
                    var resident = _context.Tbl_Residents.FirstOrDefault(r => r.CivilStatus == model.CivilStatus);
                    if (resident != null)
                    {
                        model.fullName = resident.Fullname;
                        model.BirthDate = resident.DateOfBirth;
                        // Set other required fields
                        model.DateCreated = DateTime.Now;
                        model.DateModified = DateTime.Now;
                        model.IsActive = true;

                        // Add HouseOccupantModel to database
                        _context.Tbl_HouseOccupants.Add(model);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(HouseDashboard));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Selected resident not found.");
                    }
                }
                // Repopulate the dropdown list if there's an error
                ViewBag.Residents = _context.Tbl_Residents
                    .Select(r => new SelectListItem
                    {
                        Value = r.CivilStatus.ToString(),
                        Text = $"{r.Fullname}"
                    })
                    .ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                return View(model);
            }
        }

    }
}
