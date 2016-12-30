using CodeKicker.BBCode;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ForumsController : Controller
    {

        
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Create(string category)
        {
            Post post = new Post();
            post.Category = category;
            ViewBag.Cat = category;
            return View(post);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                post.PostedDate = DateTime.Now;
                post.Author = user.DisplayName;
                var db = new ForumsDbContext();
                db.Posts.Add(post);
                 db.SaveChanges();
            }
            return RedirectToAction("ViewPost", new { id = post.Id });


        }

        [HttpGet]
        public ActionResult ViewPost(long id)
        {
            var db = new ForumsDbContext();
            var post = db.Posts.SingleOrDefault(x => x.Id == id);
            return View(post);
        }
        [HttpGet]
        public ActionResult ViewForums(string category)
        {
            ViewBag.Cat = category;
            var db = new ForumsDbContext();
            List<Post> query =
                (from hurr in db.Posts
                where hurr.Category.Equals(category)
                select hurr).ToList();
            ViewBag.MyList = query;
            return View();
        }

        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            var db = new ForumsDbContext();
            List<Post> recent =
                (from hurr in db.Posts
                 orderby hurr.PostedDate descending
                 select hurr).Take(5).ToList();
            ViewBag.Recent = recent;
            return PartialView();
        }
    }
}