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
            foreach (var interaction in topUserIds)
            {
                if (!string.IsNullOrWhiteSpace(interaction.UserId) || !string.IsNullOrWhiteSpace(interaction.PageUrl))
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

        public class UserData
        {
            public string UserId { get; set; } = string.Empty;
            public string PageUrl { get; set; }
        }

        public ActionResult Line()
        {
            var barChartData = _context.CurrentInteractions
    .Where(ci => ci.PageUrl != "http://localhost:24019/Account/Login" && ci.PageUrl != "http://localhost:24019/Account/Register")
    .GroupBy(ci => ci.PageUrl)
    .Select(group => new CountData
    {
        PageUrl = group.Key,
        Count = group.Count()
    })
                .ToList();

            var pageUrlArray = barChartData.Select(item => item.PageUrl).ToArray();
            var countArray = barChartData.Select(item => item.Count).ToArray();

            ViewBag.PageUrlData = pageUrlArray;
            ViewBag.CountData = countArray;

            return View();
        }


        public class CountData
        {
            public int Count { get; set; }
            public string PageUrl { get; set; }
        }


        public ActionResult Pie()
        {

            return View();
        }


        public ActionResult Area()
        {

            return View();
        }

        public ActionResult Bar()
        {

            return View();
        }
        public ActionResult All()
        {

            return View();
        }

        //public IActionResult Privacy()
        //{
        //    var privacyPageCounts = _context.CurrentInteractions.FromSqlRaw("exec dbo.getNavigatedUserCount").ToList();

        //    List<PageCount> privacyPageList = new List<PageCount>();
        //    foreach (var interaction in privacyPageCounts)
        //    {
        //        if (!string.IsNullOrWhiteSpace(interaction.USERCOUNT) && !string.IsNullOrWhiteSpace(interaction.PageUrl))
        //        {
        //            var userCountValue = interaction.USERCOUNT;

        //            var pageCount = new PageCount
        //            {
        //                USERCOUNT = userCountValue,
        //                PageUrl = interaction.PageUrl
        //            };
        //            privacyPageList.Add(pageCount);
        //        }
        //    }

        //    var privacyPageCountsDict = new Dictionary<string, string>();
        //    foreach (var pageCount in privacyPageList)
        //    {
        //        privacyPageCountsDict.Add(pageCount.USERCOUNT, pageCount.PageUrl);
        //    }

        //    return Json(privacyPageCountsDict);
        //}


        public class PageCount
        {
            public string USERCOUNT { get; set; } = string.Empty;
            public string PageUrl { get; set; }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
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