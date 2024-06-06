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
using MargotCodeSystem.Database.DbModels.ResidentModels;
using X.PagedList;

namespace MargotCodeSystem.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    public class ResidentController(MargotCodeSystemDbContext context) : Controller
    {
        private readonly MargotCodeSystemDbContext _context = context;

        [HttpGet]
        public IActionResult Dashboard(int? page, bool? seniorCitizen, bool? streetSweeper, bool? activeResident, bool? medicationUser, bool? petOwner)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the total number of residents
            int totalResidents = _context.Tbl_Residents.Count();

            var dashboardQuery = _context.Tbl_Dashboard
                .Where(d => d.UserId == userId && d.IsActive == true);

            // Apply filters
            if (seniorCitizen.HasValue && seniorCitizen.Value)
            {
                dashboardQuery = dashboardQuery.Where(d => d.seniorCitizen == seniorCitizen.Value);
            }

            if (streetSweeper.HasValue && streetSweeper.Value)
            {
                dashboardQuery = dashboardQuery.Where(d => d.streetSweeper == streetSweeper.Value);
            }

            if (activeResident.HasValue && activeResident.Value)
            {
                dashboardQuery = dashboardQuery.Where(d => d.activeResident == activeResident.Value);
            }

            if (medicationUser.HasValue && medicationUser.Value)
            {
                dashboardQuery = dashboardQuery.Where(d => d.medicationUser == medicationUser.Value);
            }

            if (petOwner.HasValue && petOwner.Value)
            {
                dashboardQuery = dashboardQuery.Where(d => d.petOwner == petOwner.Value);
            }

            var dashboardList = dashboardQuery
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
                .ToPagedList(pageNumber, pageSize); // Convert to IPagedList here

            // Pass the total number of residents to the view
            ViewBag.TotalResidents = totalResidents;

            // Pass filters to view for form state retention
            ViewBag.SeniorCitizen = seniorCitizen;
            ViewBag.StreetSweeper = streetSweeper;
            ViewBag.ActiveResident = activeResident;
            ViewBag.MedicationUser = medicationUser;
            ViewBag.PetOwner = petOwner;

            return View(dashboardList);
        }




        //Get Adding Resident View
        [HttpGet]
        public IActionResult AddResident()
        {
            return View();
        }

        //Resident Count 



        //Adding Resident Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResident(ResidentModel model, List<string> petNames, List<string> medNames, List<EmployeeModel> employees)
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


                if (petNames != null && petNames.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    foreach (var petName in petNames)
                    {
                        var pet = new PetModel
                        {
                            Name = EncryptionHelper.EncryptString(petName),
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true,
                            ResidentId = model.Id
                        };
                        _context.Tbl_Pets.Add(pet);
                    }
                }



                if (medNames != null && medNames.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    foreach (var medName in medNames)
                    {
                        var med = new MedsModel
                        {
                            Name = EncryptionHelper.EncryptString(medName),
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            IsActive = true,
                            ResidentId = model.Id
                        };
                        _context.Tbl_Meds.Add(med);
                    }
                }

                if (employees != null)
                {
                    foreach (var emp in employees)
                    {
                        // Check if any of the properties of the EmployeeModel are null or empty
                        if (!string.IsNullOrWhiteSpace(emp.EmployeeDuration) &&
                            !string.IsNullOrWhiteSpace(emp.CompanyName) &&
                            !string.IsNullOrWhiteSpace(emp.Employer))
                        {
                            // Encrypt the properties if they are not null or empty
                            emp.EmployeeDuration = EncryptionHelper.EncryptString(emp.EmployeeDuration);
                            emp.CompanyName = EncryptionHelper.EncryptString(emp.CompanyName);
                            emp.Employer = EncryptionHelper.EncryptString(emp.Employer);
                            emp.DateCreated = DateTime.Now;
                            emp.DateModified = DateTime.Now;
                            emp.IsActive = true;
                            emp.ResidentId = model.Id;

                            _context.Tbl_Employee.Add(emp);
                        }
                    }
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
            return RedirectToAction("Dashboard", "Resident");
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

            var residentPets = _context.Tbl_Pets.Where(p => p.ResidentId == id).ToList();
            var residentMeds = _context.Tbl_Meds.Where(m => m.ResidentId == id).ToList();
            var residentEmployee = _context.Tbl_Employee.Where(m => m.ResidentId == id).ToList();

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

            foreach (var pet in residentPets)
            {
                pet.Name = EncryptionHelper.DecryptString(pet.Name);
            }

            foreach (var med in residentMeds)
            {
                med.Name = EncryptionHelper.DecryptString(med.Name);
            }

            foreach (var employee in residentEmployee)
            {
                employee.EmployeeDuration = EncryptionHelper.DecryptString(employee.EmployeeDuration);
                employee.Employer = EncryptionHelper.DecryptString(employee.Employer);
                employee.CompanyName = EncryptionHelper.DecryptString(employee.CompanyName);
            }

            var viewModel = new ResidentViewModel
            {
                Resident = resident,
                HouseoccupantGroup = occupantGroups,
                Pets = residentPets,
                Meds = residentMeds,
                Employee = residentEmployee
            };
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult EditResident(int id)
        {
            var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var residentPets = _context.Tbl_Pets.Where(p => p.ResidentId == id).ToList();
            var residentMeds = _context.Tbl_Meds.Where(m => m.ResidentId == id).ToList();
            var residentEmployee = _context.Tbl_Employee.Where(m => m.ResidentId == id).ToList();

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

            foreach (var pet in residentPets)
            {
                pet.Name = EncryptionHelper.DecryptString(pet.Name);
            }

            foreach (var med in residentMeds)
            {
                med.Name = EncryptionHelper.DecryptString(med.Name);
            }

            foreach (var employee in residentEmployee)
            {
                employee.EmployeeDuration = EncryptionHelper.DecryptString(employee.EmployeeDuration);
                employee.Employer = EncryptionHelper.DecryptString(employee.Employer);
                employee.CompanyName = EncryptionHelper.DecryptString(employee.CompanyName);
            }

            var viewModel = new ResidentViewModel
            {
                Resident = resident,
                HouseoccupantGroup = occupantGroups,
                Pets = residentPets,
                Meds = residentMeds,
                Employee = residentEmployee
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditResident(ResidentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Update Resident model
                    var residentToUpdate = _context.Tbl_Residents.FirstOrDefault(r => r.Id == viewModel.Resident.Id);
                    if (residentToUpdate != null)
                    {
                        // Update resident properties
                        residentToUpdate.LastName = viewModel.Resident.LastName;
                        residentToUpdate.FirstName = viewModel.Resident.FirstName;
                        residentToUpdate.MiddleName = viewModel.Resident.MiddleName;
                        residentToUpdate.Gender = viewModel.Resident.Gender;
                        residentToUpdate.CivilStatus = viewModel.Resident.CivilStatus;
                        residentToUpdate.DateOfBirth = viewModel.Resident.DateOfBirth;
                        residentToUpdate.PlaceOfBirth = viewModel.Resident.PlaceOfBirth;
                        residentToUpdate.Religion = viewModel.Resident.Religion;
                        residentToUpdate.Weight = viewModel.Resident.Weight;
                        residentToUpdate.Height = viewModel.Resident.Height;
                        residentToUpdate.HouseType = viewModel.Resident.HouseType;
                        residentToUpdate.LengthOfStay = viewModel.Resident.LengthOfStay;
                        residentToUpdate.Registered = viewModel.Resident.Registered;
                        residentToUpdate.PrecintNumber = viewModel.Resident.PrecintNumber;
                        residentToUpdate.Remarks = viewModel.Resident.Remarks;

                        _context.SaveChanges();
                    }

                    // Update related entities if needed
                    // For example, update pets, meds, employees, etc.

                    return RedirectToAction("ViewResident", new { id = viewModel.Resident.Id });
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    ViewBag.ErrorMessage = "An error occurred while processing your request. Please try again later.";
                    return View("ViewResident", viewModel); // Return the view with the same data for editing
                }
            }
            else
            {
                // If ModelState is not valid, return to the view with validation errors
                return View("ViewResident", viewModel);
            }
        }


        [HttpGet]
        public IActionResult PrintDetails(int id)
        {
            var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var residentPets = _context.Tbl_Pets.Where(p => p.ResidentId == id).ToList();
            var residentMeds = _context.Tbl_Meds.Where(m => m.ResidentId == id).ToList();
            var residentEmployee = _context.Tbl_Employee.Where(m => m.ResidentId == id).ToList();

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

            foreach (var pet in residentPets)
            {
                pet.Name = EncryptionHelper.DecryptString(pet.Name);
            }

            foreach (var med in residentMeds)
            {
                med.Name = EncryptionHelper.DecryptString(med.Name);
            }

            foreach (var employee in residentEmployee)
            {
                employee.EmployeeDuration = EncryptionHelper.DecryptString(employee.EmployeeDuration);
                employee.Employer = EncryptionHelper.DecryptString(employee.Employer);
                employee.CompanyName = EncryptionHelper.DecryptString(employee.CompanyName);
            }

            var viewModel = new ResidentViewModel
            {
                Resident = resident,
                HouseoccupantGroup = occupantGroups,
                Pets = residentPets,
                Meds = residentMeds,
                Employee = residentEmployee
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

        [HttpGet]
        public IActionResult RecoverView()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dashboardList = _context.Tbl_Dashboard
                .Where(d => d.UserId == userId && d.IsActive == false)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RecoverResident(int id)
        {
            var resident = _context.Tbl_Residents.FirstOrDefault(r => r.Id == id);
            if (resident == null)
            {
                return NotFound();
            }

            // Soft delete: Set IsActive to false instead of removing from database
            resident.IsActive = true;
            _context.SaveChanges();

            // Update corresponding dashboard entry
            var dashboardEntry = _context.Tbl_Dashboard.FirstOrDefault(d => d.ResidentId == id);
            if (dashboardEntry != null)
            {
                dashboardEntry.IsActive = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Resident");
        }


    }
}