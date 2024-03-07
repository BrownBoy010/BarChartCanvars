using BarChartCanvars.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BarChartCanvars.Controllers
{
    public class HomeController : Controller
    {
        private readonly InteractionsContext _context;

        public HomeController(InteractionsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetTopUserIds()
        {
            var topUserIds = _context.CurrentInteractions
                                .OrderByDescending(i => i.Id) 
                                .Select(i => i.UserId)
                                .Take(6)
                                .ToList();

            return Json(topUserIds);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
