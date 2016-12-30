using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ForumsDbContext();
            List<Post> query =
                (from hurr in db.Posts
                 where hurr.Category.Equals("News-and-Announcements")
                 orderby hurr.PostedDate descending
                 select hurr).ToList();
            ViewBag.News = query;
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.TheMessage = "Send us a Message";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.TheMessage = "Got your message";
            return View();
        }
        public ActionResult Minecraft()
        {
            return View();
        }

        public ActionResult Users()
        {
            var db = new ApplicationDbContext();
            List<UserStats> allUsers =
                (from hurr in db.UserStat
                 orderby hurr.DisplayName
                 select hurr).ToList();
            ViewBag.Users = allUsers;
            return View();
        }

        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            return PartialView();
        }
    }
}