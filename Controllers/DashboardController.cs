using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
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

        //Change to default as sending the partialView with model, so we dont need to call two methods when returning back to activity tab
        [HttpGet]
        [ProfileViewCount]
        public ActionResult Index(string id)
        {
            ViewBag.name = id;
            {
                //Retrive user wall post and display on index
                UserInfoViewModel model = new UserInfoViewModel();
                model.User = db.UserStat.SingleOrDefault(x => x.DisplayName == id);
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
                //Fetch Users level and Website tags information
                UserTags tag = db.Tags.SingleOrDefault(x => x.TagName == model.User.TagGroup);
                Levels level = db.UserLevel.SingleOrDefault(x => x.LevelName == model.User.ProfileLevel);
                model.UserTag = tag;
                model.UserLevel = level;
                model.NumFriends = friendList.Count;
                //Fetch the most recent wall post made and pass them as a list
                //To Do- Pagination cant be done with PagedList as one model is already passed, but we can use ajax to fetch more replies if the bottom is reached and areMore == true
                List<WallPostViewModel> post = new List<WallPostViewModel>();
                List<WallPost> wallpost =
                    (from x in db.WallPost
                     where x.ownerId.Equals(model.User.ApplicationUserId)
                     orderby x.TimePosted descending
                     select x).Take(5).ToList();
                foreach (var item in wallpost)
                {
                    WallPostViewModel wallpostItem = new WallPostViewModel();
                    var userstat = db.UserStat.Where(x => x.ApplicationUserId == item.posterId).SingleOrDefault();
                    BasicUserViewModel poster = new BasicUserViewModel { Name = userstat.DisplayName, ProfileUrl = userstat.ProfilePicture, Effects = userstat.NameEffect, Tags = userstat.ForumsGroup };
                    wallpostItem.post = item;
                    wallpostItem.user = poster;
                    List<BasicReplyViewModel> allReplies = new List<BasicReplyViewModel>();
                    List<WallPostComment> reply =
                        (from z in db.WallPostComment
                         where z.WallPostId.Equals(item.Id)
                         orderby z.PostedDate ascending
                         select z).ToList();
                    foreach(var r in reply)
                    {
                        UserStats temp = db.UserStat.Where(x => x.ApplicationUserId == r.Author).SingleOrDefault();
                        BasicReplyViewModel hurr = new BasicReplyViewModel { Body = r.Body, Owner = r.Author, TimePosted = r.PostedDate, Author= temp.DisplayName, Profile =  temp.ProfilePicture};
                        allReplies.Add(hurr);
                    }
                    wallpostItem.replies = allReplies;
                    post.Add(wallpostItem);
                }
                ViewBag.Post = post;

                ViewBag.Friends = userFriends;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Dashboard/Partials/Index.cshtml");
                }
                if (!model.User.DisplayName.Equals(user.DisplayName))
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

        public void WallPost(string hurr, string user)
        {
            //Create a wallpost object and save it, prepare html for ajax to dynamically load into top of list
            UserStats opie = db.UserStat.Where(x => x.ApplicationUserId == user).SingleOrDefault();
            var name = User.Identity.GetUserId();
            UserStats poster = db.UserStat.Where(x => x.ApplicationUserId == name).SingleOrDefault();
            WallPost post = new WallPost {  ownerId = opie.ApplicationUserId, postBody = hurr, TimePosted = DateTime.Now, posterId = poster.ApplicationUserId, posterName = poster.DisplayName };
            db.WallPost.Add(post);
            db.SaveChanges();
        }
        public void WallReply(string hurr, long wallPostId)
        {
            var name = User.Identity.GetUserId();
            UserStats user = db.UserStat.Where(x => x.ApplicationUserId == name).SingleOrDefault();
            WallPostComment reply = new WallPostComment { Body = hurr, Author = name, WallPostId = wallPostId, PostedDate = DateTime.Now, Name = user.DisplayName};
            db.WallPostComment.Add(reply);
            db.SaveChanges();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Index(ImageViewModel model, string id)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
            {
                return View();
            }
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
            //Retrive t most recent post or comments made
            if (Request.IsAjaxRequest())
            {
                //Gather and return ActivityViewModel for recent activity            
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
            //Send basic user details
            
            if (Request.IsAjaxRequest())
            {
                //Gather and return ViewModel containing users information, add filter to remove info that the user sets to private or friends only-TODO
                UserStats user = db.UserStat.Where(x => x.DisplayName == id).SingleOrDefault();
                AccountBasicViewModel model = new AccountBasicViewModel { NickName = user.NickName, Quote = user.Quote, Bio = user.Bio, Location = user.Location};
                return PartialView("~/Views/Dashboard/Partials/Info.cshtml", model);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult Pictures(string id)
        {
            //Retrive user pictures, check if the user is the owner, if so display options for editing
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
            //Return pagination of all post started, not comments made
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
        public ActionResult ImageEdit(string id, string data)
        {
            if (Request.IsAjaxRequest())
            {
                ImageViewModel model = new ImageViewModel { Type = data, user = id };
                ViewBag.Type = data;
                ViewBag.userName = id;
                return PartialView("~/Views/Dashboard/Partials/ImageUploader.cshtml", model);
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
        public void GetUserLevel(int exp)
        {
            Levels level =
                (from hurr in db.UserLevel
                 where hurr.ExpNeeded > exp
                 orderby hurr.ExpNeeded ascending
                 select hurr).SingleOrDefault();
            ViewBag.Level = level;
        }
        public ActionResult UserLevel(string name)
        {
            //Gather relavent info and generate html to return
            int exp =
                (from hurr in db.UserStat
                 where hurr.DisplayName.Equals(name)
                 select hurr.ProfileExp).SingleOrDefault();
            List<Levels> levels = (from hurr in db.UserLevel
                                   orderby hurr.ExpNeeded ascending
                                   select hurr).ToList();
            //Loop to figure out which level the user sits in by comparing current level to required xp
            Levels temp = new Levels();
            var prevExp = 0;
            foreach (Levels level in levels)
            {
                if(exp > level.ExpNeeded)
                {
                    temp = level;
                    prevExp = level.ExpNeeded;
                }
                if(exp < level.ExpNeeded)
                {
                    if (temp.Id != 0)
                    {
                        temp = level;
                        break;
                    } 
                }
            }
            UpdateLevelViewModel model = new UpdateLevelViewModel { Name = temp.LevelName, Color = temp.LevelColor, Exp = exp, Effects = temp.LevelEffects, LevelExp = temp.ExpNeeded, PrevExp = prevExp};
            return PartialView("~/Views/Dashboard/Partials/UserLevelPartial.cshtml", model);
        }
    }
}