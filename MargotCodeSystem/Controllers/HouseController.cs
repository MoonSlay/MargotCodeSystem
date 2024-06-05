using MargotCodeSystem.Database;
using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Utils;
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
                        FullName = EncryptionHelper.DecryptString(o.FullName),
                        Position = EncryptionHelper.DecryptString(o.Position),
                        Age = EncryptionHelper.DecryptString(o.Age),
                        BirthDate = EncryptionHelper.DecryptString(o.BirthDate),
                        CivilStatus = EncryptionHelper.DecryptString(o.CivilStatus),
                        SourceIncome = EncryptionHelper.DecryptString(o.SourceIncome)
                    }).ToList()
                })
                .ToList();

            return View(occupantGroups);
        }

        //Get Adding House View
        [HttpGet]
        public IActionResult AddHouse()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Viewbag for dropdown list data from Residents
                ViewBag.Residents = _context.Tbl_Residents
                    .Where(r => !_context.Tbl_HouseGroup.Any(hg => hg.ResidentId == r.Id))
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.Fullname)}"
                    });

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
                    var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == model.ResidentId);
                    if (resident != null)
                    {
                        model.FullName = EncryptionHelper.DecryptString(resident.Fullname);
                        model.CivilStatus = resident.CivilStatus;
                        model.BirthDate = resident.DateOfBirth;
                        // Set other required fields
                        model.DateCreated = DateTime.Now;
                        model.DateModified = DateTime.Now;
                        model.IsActive = true;
                        model.UserId = user.Id;
                        model.ResidentId = resident.Id;

                        // Add HouseOccupantModel to database
                        _context.Tbl_HouseOccupants.Add(model);
                        _context.SaveChanges();

                        var House = new HouseOccupantGroupModel
                        {
                            HouseName = model.HouseName,
                            UserId = user.Id,
                            ResidentId = resident.Id,
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

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Populate ViewBag.Residents with data for dropdown list registered by the specific user
                ViewBag.Residents = _context.Tbl_Residents
                    .Where(r => !_context.Tbl_HouseGroup.Any(hg => hg.ResidentId == r.Id))
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.Fullname)}"
                    });
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Repopulate the dropdown list if there's an error
                ViewBag.Residents = _context.Tbl_Residents
                    .Where(r => !_context.Tbl_HouseGroup.Any(hg => hg.ResidentId == r.Id))
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.Fullname)}"
                    });

                ViewBag.House = _context.Tbl_HouseGroup
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.HouseName.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.HouseName)}"
                    })
                    // To only display Same Housename Once
                    .Distinct();

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
                    var user = _context.Tbl_ApplicationUsers
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);

                    // Retrieve the selected resident
                    var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == model.ResidentId);
                    if (resident != null)
                    {
                        model.FullName = EncryptionHelper.DecryptString(resident.Fullname);
                        model.CivilStatus = resident.CivilStatus;
                        model.BirthDate = resident.DateOfBirth;
                        // Set other required fields
                        model.DateCreated = DateTime.Now;
                        model.DateModified = DateTime.Now;
                        model.IsActive = true;
                        model.UserId = user.Id; // Set the UserId to the ID of the logged-in user
                        model.ResidentId = resident.Id;

                        // Add HouseOccupantModel to database
                        _context.Tbl_HouseOccupants.Add(model);
                        _context.SaveChanges();

                        model.HouseName = EncryptionHelper.DecryptString(model.HouseName);
                        var House = new HouseOccupantGroupModel
                        {
                            HouseName = model.HouseName,
                            UserId = user.Id,
                            ResidentId = resident.Id,
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Populate ViewBag.Residents with data for dropdown list
                ViewBag.Residents = _context.Tbl_Residents
                    .Where(r => !_context.Tbl_HouseGroup.Any(hg => hg.ResidentId == r.Id))
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.Fullname)}"
                    });

                ViewBag.House = _context.Tbl_HouseGroup
                    .Where(r => r.UserId == userId) // Filter by UserId
                    .Select(r => new SelectListItem
                    {
                        Value = r.HouseName.ToString(),
                        Text = $"{EncryptionHelper.DecryptString(r.HouseName)}"
                    })
                    .Distinct();

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
