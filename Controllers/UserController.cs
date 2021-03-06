﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult AddFriend(string id)
        {
            if (Request.IsAjaxRequest())
            {
                var db = new ApplicationDbContext();
                ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                UserStats friendUser =
                    (from hurr in db.UserStat
                     where hurr.DisplayName.Equals(id)
                     select hurr).SingleOrDefault();
                var friend = new Friends();
                friend.userId = currentUser.Id;
                friend.friendId = friendUser.ApplicationUserId;
                db.Friend.Add(friend);
                db.SaveChanges();
                friend.userId = friendUser.ApplicationUserId;
                friend.friendId = currentUser.Id;
                db.Friend.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return null;
            }

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
        public ActionResult Refer()
        {

            return PartialView("~/Views/Shared/Partials/ReferView.cshtml");
        }

        public string GetIdFromName(string name)
        {
            var Id = db.UserStat.Where(x => x.DisplayName == name).SingleOrDefault();
            return Id.ApplicationUserId;
        }
        public void AwardUser(FriendRefer refer)
        {
            
        }
    }
}