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

        public IActionResult GetLiveUsers()
        {
            var liveUsers = _context.UserLoginDetails
                .Where(c => c.Logout == null).ToList();

            return View(liveUsers);
        }

        public class UserData
        {
            public string? UserId { get; set; } = string.Empty;
            public string? PageUrl { get; set; }
        }

        public ActionResult PageURLvsCount()
        {
            var barChartData = _context.CurrentInteractions
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


        public ActionResult ErrorPage()
        {
            DateTime LastFiveDays = DateTime.Today.AddDays(-10);

            var query = from interaction in _context.CurrentInteractions
                        where interaction.PageUrl == "error" &&
                              interaction.SaveDateTime.HasValue &&
                              interaction.SaveDateTime.Value.Date >= LastFiveDays
                        group interaction by interaction.SaveDateTime.Value.Date into g
                        select new
                        {
                            InteractionCount = g.Count(),
                            InteractionDate = g.Key
                        };

            var CountArray = query.Select(data => data.InteractionCount).ToArray();
            var DateArray = query.Select(data => data.InteractionDate.ToString("yyyy-MM-dd")).ToArray();

            ViewBag.CountData = CountArray;
            ViewBag.DateData = DateArray;


            return View();
        }


        public ActionResult AgentCount()
        {

            var result = _context.CurrentInteractions.Select(interaction => new
            {
                UserID = interaction.UserId,
                ModifiedUserAgent = interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0" ? "Edge" :
                                (interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36" ? "Chrome" : interaction.UserAgent)
            })
            .Distinct()
            .GroupBy(x => x.ModifiedUserAgent)
            .Select(g => new
            {
                UserAgent = g.Key,
                UserAgentCount = g.Count()
            });

            var UserAgentArray = result.Select(data => data.UserAgent).ToArray();
            var UserAgentCountArray = result.Select(data => data.UserAgentCount).ToArray();

            ViewBag.UserAgentData = UserAgentArray;
            ViewBag.UserAgentCountData = UserAgentCountArray;

            //error count 3D
            var ErrorCount = from interaction in _context.CurrentInteractions
                             where interaction.PageUrl == "error"
                             let modifiedUserAgent =
                                 interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0" ? "Edge" :
                                 interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36" ? "Chrome" :
                                 interaction.UserAgent
                             group interaction by modifiedUserAgent into g
                             select new
                             {
                                 ErrorUserAgent = g.Key,
                                 ErrorAgentCount = g.Count(),
                                 ErrorUserCount = g.Select(interaction => interaction.UserId).Distinct().Count()
                             };

            var ErrorUserAgentArray = ErrorCount.Select(data => data.ErrorUserAgent).ToArray();
            var ErrorAgentCountArray = ErrorCount.Select(data => data.ErrorAgentCount).ToArray();
            var ErrorUserCountArray = ErrorCount.Select(data => data.ErrorUserCount).ToArray();

            ViewBag.ErrorUserAgentData = ErrorUserAgentArray;
            ViewBag.ErrorAgentCountData = ErrorAgentCountArray;
            ViewBag.ErrorUserCountData = ErrorUserCountArray;


            //Success count 3D
            var SuccessCount = from interaction in _context.CurrentInteractions
                               where interaction.PageUrl != "error"
                               let modifiedUserAgent =
                                   interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0" ? "Edge" :
                                   interaction.UserAgent == "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36" ? "Chrome" :
                                   interaction.UserAgent
                               group interaction by modifiedUserAgent into g
                               select new
                               {
                                   SuccessUserAgent = g.Key,
                                   SuccessAgentCount = g.Count(),
                                   SuccessUserCount = g.Select(interaction => interaction.UserId).Distinct().Count()
                               };

            var SuccessUserAgentArray = SuccessCount.Select(data => data.SuccessUserAgent).ToArray();
            var SuccessAgentCountArray = SuccessCount.Select(data => data.SuccessAgentCount).ToArray();
            var SuccessUserCountArray = SuccessCount.Select(data => data.SuccessUserCount).ToArray();

            ViewBag.SuccessUserAgentData = SuccessUserAgentArray;
            ViewBag.SuccessAgentCountData = SuccessAgentCountArray;
            ViewBag.SuccessUserCountData = SuccessUserCountArray;


            return View();
        }

        public ActionResult Bar()
        {

            return View();
        }
        public ActionResult DashBoard()
        {
            var query = from interaction in _context.CurrentInteractions
                        where interaction.PageUrl == "error" &&
                              interaction.SaveDateTime.HasValue
                        group interaction by interaction.SaveDateTime.Value.Date into g
                        select new
                        {
                            InteractionCount = g.Count(),
                            InteractionDate = g.Key
                        };

            var CountArray = query.Select(data => data.InteractionCount).ToArray();
            var DateArray = query.Select(data => data.InteractionDate.ToString("yyyy-MM-dd")).ToArray();

            ViewBag.CountData = CountArray;
            ViewBag.DateData = DateArray;


            //last drop off
            var queryy = from interaction in _context.CurrentInteractions
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

            var PageUrlArray = queryy.Select(data => data.PageUrl).ToArray();
            var UserCountArray = queryy.Select(data => data.UserCount).ToArray();


            ViewBag.PageUrlData = PageUrlArray;
            ViewBag.UserCountData = UserCountArray;


            //
            var barChartData = _context.CurrentInteractions
                .GroupBy(ci => ci.PageUrl)
                .Select(group => new
                {
                    PageUrl = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(data => data.Count)
                .ToList();

            var pageUrlArray1 = barChartData.Select(item => item.PageUrl).ToArray();
            var countArray1 = barChartData.Select(item => item.Count).ToArray();

            ViewBag.PageUrlData1 = pageUrlArray1;
            ViewBag.CountData1 = countArray1;


            return View();
        }



    }


}

