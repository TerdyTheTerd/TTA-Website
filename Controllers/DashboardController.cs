﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {

        [HttpGet]
        public ActionResult Index(string id)
        {
            ViewBag.name = id;
            //var db = new ApplicationDbContext();
            {
                UserStats model = db.UserStat.SingleOrDefault(x => x.DisplayName == id);
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                //string profileURL = "/../Assets/UserProfilePics/" + id + "-Profile.png";
                string profileURL = (from hurr in db.UserStat
                                     where hurr.DisplayName.Equals(id)
                                     select hurr.ProfilePicture).FirstOrDefault();
                ViewBag.Profile = profileURL;
                string bannerURL = (from hurr in db.UserStat
                                    where hurr.DisplayName.Equals(id)
                                    select hurr.ProfileBanner).SingleOrDefault();
                ViewBag.Banner = bannerURL;
                string userID = (
                    from hurr in db.UserStat
                    where hurr.DisplayName.Equals(id)
                    select hurr.ApplicationUserId).SingleOrDefault();
                List<string> friendList =
                    (from hurr in db.Friend
                     where hurr.userId.Equals(userID)
                     select hurr.friendId).ToList();
                List<UserStats> userFriends = new List<UserStats>();

                foreach (string name in friendList)
                {
                    UserStats friend =
                            (from hurr in db.UserStat
                             where hurr.ApplicationUserId.Equals(name)
                             select hurr).SingleOrDefault();
                    if (!(friend == null))
                    {
                        userFriends.Add(friend);
                    }

                }
                ViewBag.Friends = userFriends;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Dashboard/Partials/Index.cshtml");
                }
                if (!model.DisplayName.Equals(user.DisplayName))
                {
                    if (friendList.Contains(user.Id))
                    {
                        ViewBag.isFriend = true;
                    }
                    else
                    {
                        ViewBag.isFriend = false;
                    }
                    return View(model);
                }
                else
                {
                    ViewBag.isFriend = false;
                    return View(model);
                }

            }
        }

        [HttpPost]
        public ActionResult Index(ImageViewModel model, string id)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View();
            }
            //ApplicationDbContext db = new ApplicationDbContext();
            UserStats user =
                (from hurr in db.UserStat
                 where hurr.DisplayName.Equals(id)
                 select hurr).SingleOrDefault();
            string filename;
            string path = "";
            int srcWidth = 0;
            int srcHeight = 0;
            switch (model.Type)
            {
                case "banner":
                    ;
                    filename = user.DisplayName + "-Banner.png";
                    path = "~/Assets/UserBannerPics/" + filename;
                    user.ProfileBanner = "/../Assets/UserBannerPics/" + filename;
                    srcWidth = 900;
                    srcHeight = 175;
                    break;
                case "profile":
                    ;
                    filename = user.DisplayName + "-Profile.png";
                    path = "~/Assets/UserProfilePics/" + filename;
                    user.ProfilePicture = "/../Assets/UserProfilePics/" + filename;
                    srcWidth = srcHeight = 175;
                    break;
            }
            float scaleX = ((float)srcWidth / model.width);
            float scaleY = ((float)srcHeight / model.height);
            //Create the path for the file
            var fullpath = Request.MapPath(path);
            //Call a method to crop the base image using parameters from form
            // Update the UserStats row of corresponding user with the new, validated picture path
            db.SaveChanges();
            try
            {
                Image original = Image.FromStream(model.File.InputStream, true, true);
                Rectangle cropArea = new Rectangle(model.offSetX, model.offSetY, model.width, model.height);
                Rectangle final = new Rectangle(0, 0, model.width, model.height);
                //Bitmap sourceImage = new Bitmap(model.width, model.height);
                using (var sourceImage = new Bitmap(srcWidth, srcHeight))
                {
                    using (Graphics g = Graphics.FromImage(sourceImage))
                    {
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.ScaleTransform(scaleX, scaleY);
                        g.DrawImage(original, final, cropArea, GraphicsUnit.Pixel);
                    }
                    sourceImage.Save(fullpath, ImageFormat.Png);
                }

                //cropImage(model).Save(fullpath);
            }
            catch (FileNotFoundException e)
            {
                e.GetBaseException();
            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Activity(string id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Dashboard/Partials/Activity.cshtml");
            }
            else
            {
                return null;
            }
            //var db = new ApplicationDbContext();
            //UserStats model = db.UserStat.SingleOrDefault(x => x.DisplayName == id);
            //return View(model);

        }

        [HttpGet]
        public ActionResult Info(string id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Dashboard/Partials/Info.cshtml");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult Pictures(string id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Dashboard/Partials/Pictures.cshtml");
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult Post(string id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Dashboard/Partials/Post.cshtml");
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult Account(string id)
        {
            var db = new ApplicationDbContext();
            UserStats model = db.UserStat.SingleOrDefault(x => x.DisplayName == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Account(AccountBasicViewModel model)
        {
            // Add Code to save changes made
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var db = new ApplicationDbContext();
                var id = User.Identity.GetUserId();
                UserStats user = db.UserStat.SingleOrDefault(x => x.ApplicationUserId == id);
                user.NickName = model.NickName;
                user.Bio = model.Bio;
                user.Quote = model.Quote;
                user.Location = model.Location;
                db.SaveChanges();
            }


            return View("Account", model);
        }
        public ActionResult Inbox()
        {
            return View();
        }
        public ActionResult Compose()
        {
            return View();
        }
        public ActionResult Friends()
        {
            return View();
        }
        public ActionResult Points()
        {
            return View();
        }

        [ChildActionOnly]
        public string GetUser()
        {
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                return user.DisplayName;
            }
        }

        [HttpGet]
        public ActionResult ImageEdit(string id, string type)
        {
            if (Request.IsAjaxRequest())
            {
                ViewBag.Type = type;
                ViewBag.userName = id;
                return PartialView("~/Views/Dashboard/Partials/ImageUploader.cshtml");
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult AddFriend(string id)
        {
            //if (Request.IsAjaxRequest())
            //{
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
            return RedirectToAction("Index", new { id = id });
            //}
            //else
            //{
            //    return null;
            //}

        }
        public ActionResult UploadFile(PictureViewModel model)
        {
            //To Do- Handle picture uplaods from users, validate them, check user storage, store image in root with a path stored in db table
            return null;
        }
    }
}