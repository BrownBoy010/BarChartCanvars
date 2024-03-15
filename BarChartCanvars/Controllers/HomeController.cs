using BarChartCanvars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public ActionResult PageURLvsCount()
        {
            var barChartData = _context.CurrentInteractions
                .Where(ci => ci.PageUrl != "http://localhost:24019/Account/Login" && ci.PageUrl != "http://localhost:24019/Account/Register")
                .GroupBy(ci => ci.PageUrl)
                .Select(group => new 
                {
                    PageUrl = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(data => data.Count)
                .ToList();

            var pageUrlArray = barChartData.Select(item => item.PageUrl).ToArray();
            var countArray = barChartData.Select(item => item.Count).ToArray();

            ViewBag.PageUrlData = pageUrlArray;
            ViewBag.CountData = countArray;

            return View();
        }

        public ActionResult LastVisit()
        {
            var query = from interaction in _context.CurrentInteractions
                        where _context.CurrentInteractions
                                .GroupBy(c => c.UserId)
                                .Select(g => g.Max(c => c.SaveDateTime))
                                .Contains(interaction.SaveDateTime)
                        group interaction by interaction.PageUrl into g
                        orderby g.Count() descending
                        select new
                        {
                            PageUrl = g.Key,
                            UserCount = g.Count()
                        };



            var PageUrlArray = query.Select(data => data.PageUrl).ToArray();
            var UserCountArray = query.Select(data => data.UserCount).ToArray();
            

            ViewBag.PageUrlData = PageUrlArray;
            ViewBag.CountData = UserCountArray;

            return View();
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}

