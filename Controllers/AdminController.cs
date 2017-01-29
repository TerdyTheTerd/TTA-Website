using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

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

        public ActionResult UpdateLevel(UpdateLevelViewModel model)
        {
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
            return null;
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