using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserManager(string id)
        {
            //Check if the user is requesting info on a user, or requesting a list of users
            if (id != null)
            {
                UserStats model =
                    (from x in db.UserStat
                     where x.ApplicationUserId.Equals(id)
                     select x).SingleOrDefault();
                return PartialView("~/Views/Admin/Partials/UserInfo.cshtml", model);
            }
            //Return ViewBags for each Website Tag
            List<UserStats> users =
                (from x in db.UserStat
                 orderby x.DisplayName ascending
                 select x).ToList();
            ViewBag.Users = users;
            return PartialView("~/Views/Admin/Partials/Users_Admin.cshtml");
        }
        [HttpGet]
        public ActionResult LevelManager(string id)
        {
            switch (id)
            {
                case "Create":
                    {
                        CreateLevelViewModel model = new CreateLevelViewModel();
                        return PartialView("~/Views/Admin/Partials/Levels/Create.cshtml", model); ;
                    }
                case "Edit":
                    {
                        ViewBag.Levels =
                            (from x in db.UserLevel
                             orderby x.ExpNeeded ascending
                             select x).ToList();
                        ViewBag.Effects =
                            (from e in db.Effect
                             orderby e.Name
                             select e).ToList();
                        return PartialView("~/Views/Admin/Partials/Levels/Edit.cshtml");
                    }
                case "Settings":
                    {
                        
                        return PartialView("~/Views/Admin/Partials/Levels.cshtml");
                    }
                    //If we got this far something didn't work, return to main page
                default:
                    {
                        
                        return PartialView("~/Views/Admin/Partials/LevelManager.cshtml");
                    }
            }

        }
        [HttpGet]
        public ActionResult ForumManager (string id)
        {
            List<Post> post =
                (from x in db.Post
                 orderby x.PostedDate descending
                 select x).ToList();

            return PartialView("~/Views/Admin/Partials/Forum.cshtml", post);
        }
        [HttpGet]
        public ActionResult WebsiteTagManager(string id)
        {
            switch (id)
            {
                case "Create":
                    {
                        CreateWebsiteTag model = new CreateWebsiteTag();
                        ViewBag.Effects =
                            (from e in db.Effect
                             orderby e.Name
                             select e).ToList();
                        return PartialView("~/Views/Admin/Partials/WTags/Create.cshtml", model); ;
                    }
                case "Edit":
                    {
                        ViewBag.Levels =
                            (from x in db.Tags
                             orderby x.TagName ascending
                             select x).ToList();
                        ViewBag.Effects =
                            (from e in db.Effect
                             orderby e.Name
                             select e).ToList();
                        return PartialView("~/Views/Admin/Partials/Levels/Edit.cshtml"); ;
                    }
                case "Settings":
                    {

                        return PartialView("~/Views/Admin/Partials/Levels.cshtml");
                    }
                default:
                    {

                        return PartialView("~/Views/Admin/Partials/LevelManager.cshtml");
                    }
            }

        }
        [HttpGet]
        public ActionResult FTagManager(string id, int? page)
        {
            switch (id)
            {
                case "Create":
                    {
                        CreateLevelViewModel model = new CreateLevelViewModel();
                        return PartialView("~/Views/Admin/Partials/Levels/Create.cshtml", model); ;
                    }
                case "Edit":
                    {
                        //Get info on levels and effects
                        //To Do- Cahnge to a view model and pass that for typed view
                        ViewBag.Levels =
                            (from x in db.UserLevel
                             orderby x.ExpNeeded ascending
                             select x).ToList();
                        ViewBag.Effects =
                            (from e in db.Effect
                             orderby e.Name
                             select e).ToList();
                        return PartialView("~/Views/Admin/Partials/Levels/Edit.cshtml"); ;
                    }
                case "Settings":
                    {

                        return PartialView("~/Views/Admin/Partials/Levels.cshtml");
                    }
                case "Delete":
                    {
                        //Create a paged list to view post made in descending chronological order
                        int itemsPerpage = 10;
                        int pageNumber = page ?? 1;


                        List<Post> model =
                            (from x in db.Post
                             orderby x.PostedDate descending
                             select x).Take(10).ToList();
                        return PartialView("~/Views/Admin/Partials/Forum/FList.cshtml", model.ToPagedList(pageNumber, itemsPerpage));
                    }
                default:
                    {

                        return PartialView("~/Views/Admin/Partials/LevelManager.cshtml");
                    }
            }

        }

        public ActionResult Delete(long id)
        {
            //Need to trigger a backup of the record to be deleted before deleting it
            //To Do- Need to verify that a post will be deleted before its comitted
            Post post = (
                from x in db.Post
                where x.Id == id
                select x).SingleOrDefault();
            db.Post.Remove(post);
            db.SaveChanges();
                
            return RedirectToAction("Index");
        }
        public ActionResult UpdateLevel(UpdateLevelViewModel model)
        {
            //Get and update values for the level being edited
            //To DO- Change to work with ajax request, and to handle several changes at once
            Levels updateModel = db.UserLevel.Where(x => x.LevelName == model.Name).SingleOrDefault();
            updateModel.ExpNeeded = model.Exp;
            updateModel.LevelName = model.Name;
            updateModel.LevelEffects = model.Effects;
            updateModel.LevelColor = model.Color;
            updateModel.EffectName = 
                (from x in db.Effect
                 where x.EffectUrl.Equals(model.Effects)
                 select x.Name).SingleOrDefault();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CreateTag(CreateWebsiteTag model)
        {
            UserTags Tag = new UserTags { TagName = model.Name };
            db.Tags.Add(Tag);
            db.SaveChanges();
            ViewBag.Message = "Level added succesfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateLevel(CreateLevelViewModel model)
        {
            Levels level = new Levels { LevelName = model.Name, ExpNeeded = model.Exp, LevelEffects = model.Effects };
            db.UserLevel.Add(level);
            db.SaveChanges();
            ViewBag.Message = "Level added succesfully.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CreateFTag(CreateLevelViewModel model)
        {
            return null;
        }
        [HttpPost]
        public ActionResult CreateWTag(CreateLevelViewModel model)
        {
            UserTags tag = new UserTags { TagName = model.Name, TagEffect = model.Effects };
            db.Tags.Add(tag);
            db.SaveChanges();
            ViewBag.Message = "Level Added Succesfully.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ForumsManager()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AutomationManager()
        {
            return View();
        }

    }
}