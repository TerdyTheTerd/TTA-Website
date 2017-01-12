using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                List<Post> query =
                    (from hurr in db.Post
                     where hurr.Category.Equals("News-and-Announcements")
                     orderby hurr.PostedDate descending
                     select hurr).ToList();
                ViewBag.News = query;
            }
            User.IsInRole("Admin");
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

        public ActionResult Support(string message)
        {
            if(!(message == null))
            {
                ViewBag.TheMessage = "Got your message";
                return View();
            }
            ViewBag.TheMessage = "There was an error, please try again";
            return View();
        }
        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            return PartialView();
        }
    }
}