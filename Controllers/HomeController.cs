using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RequireHttps]
    public class HomeController : BaseController
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

        public ActionResult Minecraft()
        {
            return View();
        }

        public ActionResult Users()
        {
            List<UserStats> allUsers =
                (from hurr in db.UserStat
                 orderby hurr.DisplayName
                 select hurr).ToList();
            ViewBag.Users = allUsers;
            return View();
        }
        [HttpGet]
        public ActionResult Support()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Support(string message)
        {
            if (!(message == null))
            {
                ViewBag.TheMessage = "Got your message";
                return View();
            } else
            {

                ViewBag.TheMessage = "There was an error, please try again";
                return View();
            }
        }
        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult Login()
        {
            return PartialView("~/Views/Shared/_LoginDropDown.cshtml", new LoginViewModel());
        }

        public ActionResult RecentUsers()
        {
            List<UserStats> model =
                (from x in db.UserStat
                 orderby x.JoinDate descending
                 select x).ToList();
            ViewBag.Users = model;
            return PartialView("~/Views/Shared/Partials/RecentUsers.cshtml");
        }
    }
}