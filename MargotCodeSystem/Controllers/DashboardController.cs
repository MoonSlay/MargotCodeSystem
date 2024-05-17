using MargotCodeSystem.Database.DbModels;
using MargotCodeSystem.Database;
using Microsoft.AspNetCore.Mvc;

namespace MargotCodeSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MargotCodeSystemDbContext _context;

        public IActionResult Index()
        {
            return View();
        }

        public DashboardController(MargotCodeSystemDbContext context)
        {
            this._context = context;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    List<DashboardModel> dashboardList = new List<DashboardModel>();
        //    var residents = _context.Tbl_Residents.ToList();

        //    if (residents != null)
        //    {

        //        foreach (var resident in residents)
        //        {
        //            var DashboardModel = new DashboardModel()
        //            {
        //                Id = resident.Id,
        //                firstName = resident.FirstName,
        //                lastName = resident.LastName,
        //                middleName = resident.MiddleName

        //            };
        //            dashboardList.Add(DashboardModel);
        //        }
        //        return View(residents);
        //    }
        //    return View();

        //}
    }
}
