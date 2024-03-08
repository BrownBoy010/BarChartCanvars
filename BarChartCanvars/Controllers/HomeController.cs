using BarChartCanvars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var topUserIds = _context.CurrentInteractions.FromSqlRaw("exec getNavigatedUser").ToList();

            List<UserData> userlist = new List<UserData>();
            foreach (var interaction in topUserIds) { 
                if(!string.IsNullOrWhiteSpace(interaction.UserId) || !string.IsNullOrWhiteSpace(interaction.PageUrl))
                {
                    var userData = new UserData
                    {
                        UserId = interaction.UserId,
                        PageUrl = interaction.PageUrl
                    };
                    userlist.Add(userData);
                }                
            }

            var userDatas = new Dictionary<string, string>();
            foreach (var user in userlist)
            {
                userDatas.Add(user.UserId, user.PageUrl);
            }
            return Json(userDatas);
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

    public class UserData
    {
        public string UserId { get; set; } = string.Empty;
        public string PageUrl { get; set; }
    }
}



//.GroupBy(i => i.UserId)
//.Select(group => new
//{
//    UserId = group.Key,
//    MaxSaveDateTime = group.Max(i => i.SaveDateTime)
//})
//.OrderByDescending(group => group.MaxSaveDateTime)
//.Take(4)
//.Select(group => group.UserId)
//.ToList();