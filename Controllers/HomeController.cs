using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[RequireHttps]
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
        public ActionResult Shop()
        {
            return View();
        }

        public ActionResult Minecraft()
        {
            return View();
        }

        public ActionResult Users()
        {
            //Retrive a list of User tags, select all users that belong to each tag, then create a list of view model OrderedUsers
            List<OrderedUsers> allUsers = new List<OrderedUsers>();
            List<UserTags> tags =
                (from hurr in db.Tags
                 orderby hurr.TagPriority descending
                 select hurr).ToList();
            foreach (var item in tags)
            {
                List<UserStats> users =
                                (from hurr in db.UserStat
                                 where hurr.TagGroup.Equals(item.TagName)
                                 orderby hurr.DisplayName
                                 select hurr).ToList();
                OrderedUsers group = new OrderedUsers();
                group.UserTag = item.TagName;
                group.user = users;
                allUsers.Add(group);
            }
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
            }
            else
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


    }
}