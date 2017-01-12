using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var db = new ApplicationDbContext();
                db.Post.Add(post);
                 db.SaveChanges();
            }
            return RedirectToAction("ViewPost", new { id = post.Id });


        }

        [HttpPost]
        [ValidateInput(false)]
        public string PreviewPost(string test)
        {
            return  Server.HtmlDecode(CodeParser.getParser().ToHtml(test));
        }

        [ViewCount]
        [HttpGet]
        public ActionResult ViewPost(long id)
        {
            var db = new ApplicationDbContext();
            var post = db.Post.SingleOrDefault(x => x.Id == id);
            return View(post);
        }
        [HttpGet]
        public ActionResult ViewForums(string category)
        {
            ViewBag.Cat = category;
            var db = new ApplicationDbContext();
            List<Post> query =
                (from hurr in db.Post
                 where hurr.Category.Equals(category)
                 orderby hurr.PostedDate descending
                select hurr).ToList();
            //ViewBag.MyList = query;
            return View(query);
        }

        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            var db = new ApplicationDbContext();
            List<Post> recent =
                (from hurr in db.Post
                 orderby hurr.PostedDate descending
                 select hurr).Take(5).ToList();
            ViewBag.Recent = recent;
            return PartialView("~/Views/Shared/Partials/_RecentPost.cshtml");
        }

        [HttpPost]
        [ChildActionOnly]
        public ActionResult PostComment()
        {
            //Get the ViewModel data and update db
            //Increment Post data for number of replies
            //Increment UserStats data for number of post 
            return null;
        }
    }
}