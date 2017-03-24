using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ForumsController : BaseController
    {


        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Create(string category)
        {
            PostViewModel post = new PostViewModel();
            post.Category = category;
            ViewBag.Cat = category;
            return View(post);
        }
        [PostPoint]
        [Authorize]
        [HttpPost]
        public ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                UserStats user = db.UserStat.Where(x => x.ApplicationUserId == id).SingleOrDefault();
                //Clean up the create statement
                var post = new Post { PostedDate = DateTime.Now, Author = user.DisplayName, Body = model.Body, Category = model.Category,
                    Title = model.Title, Views = 0, Points = 1, HasReplies = false};
                user.TotalPost += 1;
                db.Post.Add(post);
                db.SaveChanges();
                return RedirectToAction("ViewPost", new { id = post.Id });
            }
            return View();


        }

        [HttpPost]
        [ValidateInput(false)]
        public void Comment(string test, long post)
        {
            //ToDo- Change call to handle data with an ajax call to dynamically load the new comment data with an animation
            //Process and create the new entries for postcomment, then return to index with the new PagedList Model
            var id = User.Identity.GetUserId();
            UserStats user = db.UserStat.Where(x => x.ApplicationUserId == id).SingleOrDefault();
            var comment = new PostComment { PostedDate = DateTime.Now, Body = test, Points = 0, Author = user.ApplicationUserId , WallPostId = post };
            db.WallPostReply.Add(comment);
            var OpPost = db.Post.Where(x => x.Id == post).SingleOrDefault();
            if (OpPost.HasReplies)
            {
            } else
            {
                OpPost.HasReplies = true;
            }
            OpPost.Replies += 1;
            user.TotalPost += 1;
            db.SaveChanges();
            var point = new CommentPoint { ApplicationUserId = User.Identity.GetUserId(), hasVoted = false, commentId = comment.Id };
            db.CommentPoint.Add(point);
            db.SaveChanges(); //Dont know how to access to db identity generated for added comment without calling savechanges, causing doulbe savechanges which should be avoided :/
            Response.Redirect("/Forums/ViewPost/" + post);

        }

        [HttpPost]
        [ValidateInput(false)]
        public string PreviewPost(string test)
        {
            return Server.HtmlDecode(CodeParser.getParser().ToHtml(test));
        }

        [ViewCount]
        [HttpGet]
        public ActionResult ViewPost(int id, int? page)
        {
            //Create the ThreadViewModel containing original Thread as well as a list of replies
            var post = db.Post.SingleOrDefault(x => x.Id == id);
            var thread = new ThreadViewModel();
            var user = db.UserStat.SingleOrDefault(x => x.DisplayName == post.Author);
            var current = User.Identity.GetUserId();
            List<PostPoint> points =
                (from p in db.PostPoint
                 where p.ApplicationUserId.Equals(current)
                 select p).ToList();
            thread.user = user;
            thread.post = post;
            thread.point = points;
            ViewBag.OP = thread;
            List<ThreadReplyViewModel> repliesList = new List<ThreadReplyViewModel>();
            if (post.HasReplies)
            {
                //If the post has replies then query for additional ViewModel data
                int itemsPerpage = 10;
                int pageNumber = page ?? 1; //Only Create the pagelist data if there are replies to be paged

                List<long> replyList = (from hurr in db.WallPostReply
                                        where hurr.WallPostId == id
                                        select hurr.Id).ToList();
                foreach (var x in replyList)
                {
                    var reply = new ThreadReplyViewModel();
                    var z = (from y in db.WallPostReply
                             where y.Id.Equals(x)
                             select y).SingleOrDefault();
                    reply.post = z;
                    reply.user = (from y in db.UserStat
                                  where y.ApplicationUserId.Equals(z.Author)
                                  select y).SingleOrDefault();
                    reply.points = (from y in db.CommentPoint
                                    where y.commentId.Equals(x) && y.ApplicationUserId.Equals(current)
                                    select y).SingleOrDefault();
                    repliesList.Add(reply);
                }
                //Generate Vote Options html to send to view
                //Get the different options from the Options List
                //var voteType = (from x in db.CommentPoint
                //                where x.ApplicationUserId.Equals(current)
                //                select x.VoteType).ToString();
                //switch (voteType)
                //{
                //    case "Like":
                //        {
                //            string html = "<a data-ajax=\"true\" data-ajax-method=\"GET\" data-ajax-mode=\"replace\" data-ajax-update=\"#like\" href=\"/Forums/PostVote/1/@item.post.Id\"><span class=\"fa fa-thumbs-up like\"></span></a>< a data - ajax = \"true\" data - ajax - method = \"GET\" data - ajax - mode = \"replace\" data - ajax - update = \"#like\" href = \"/Forums/PostVote/0/@item.post.Id\" >< span class=\"fa fa-thumbs-down dislike\"></span></a>";

                //            break;
                //        }
                //    case "Dislike":
                //        {
                //            string html = ;

                //            break;
                //        }

                //    default:
                //        {
                //            break;
                //        }
                //}

                return View(repliesList.ToPagedList(pageNumber, itemsPerpage));

            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult ViewForums(string category)
        {
            ViewBag.Cat = category;
            List<Post> query =
                (from hurr in db.Post
                 where hurr.Category.Equals(category)
                 orderby hurr.PostedDate descending
                 select hurr).ToList();
            //ViewBag.MyList = query;
            return View(query);
        }

        

        public string PostVote(int id, int data, string type)
        {
            //Update comment with users vote, then update related tables for hasVoted values on created entries in CommentPoints table
            //Refactor to use variables to pass info for each scenario, instead of repeating similar code twice
            if (type == "post") //Is it a Post or a Comment being voted on?
            {
                var model = db.Post.Where(x => x.Id == data).SingleOrDefault();
                var userId = User.Identity.GetUserId();
                UserStats user = db.UserStat.Where(x => x.ApplicationUserId == userId).SingleOrDefault();
                UserStats OP = db.UserStat.Where(x => x.DisplayName == model.Author).SingleOrDefault();
                var vote = db.PostPoint.Where(x => x.postId == model.Id && x.ApplicationUserId == user.ApplicationUserId).SingleOrDefault();
                vote.hasVoted = true;
                if (id == 1)
                {
                    model.Points += 1;
                    OP.PointsEarned += 1;
                    vote.VoteType = "Like";
                }
                else
                {
                    model.Points -= 1;
                    OP.PointsEarned -= 1;
                    vote.VoteType = "Dislike";
                }

                if (model.Points > 0)
                {
                    db.SaveChanges();
                    return "<span id=\"points" + model.Id + ", positive\" class=\"positive\"> +" + model.Points + "</span>";
                }
                else
                {
                    db.SaveChanges();
                    return "<span id=\"points" + model.Id + ", negative\" class=\"negative\"> " + model.Points + "</span>";
                }
            } else
            {

            var model = db.WallPostReply.Where(x => x.Id == data).SingleOrDefault();
            var userId = User.Identity.GetUserId();
            UserStats user = db.UserStat.Where(x => x.ApplicationUserId == userId).SingleOrDefault();
            UserStats OP = db.UserStat.Where(x => x.ApplicationUserId == model.Author).SingleOrDefault();
            var vote = db.CommentPoint.Where(x => x.commentId == model.Id && x.ApplicationUserId == user.ApplicationUserId).SingleOrDefault();
            vote.hasVoted = true;
            //Check the type of vote cast to increase/decrease
            if (id == 1)
            {
                model.Points += 1;
                OP.PointsEarned += 1;
                vote.VoteType = "Like";
            }
            else
            {
                model.Points -= 1;
                OP.PointsEarned -= 1;
                vote.VoteType = "Dislike";
            }

            //Check if after casting the vote the points are postive or negative, and send corresponding html
            if (model.Points > 0)
            {
                db.SaveChanges();
                return "<span id=\"points" + model.Id + ", positive\" class=\"positive\"> +" + model.Points + "</span>";
            }
            else
            {
                db.SaveChanges();
                return "<span id=\"points" + model.Id + ", negative\" class=\"negative\"> " + model.Points + "</span>";
            }
            }

        }

        [ChildActionOnly]
        public ActionResult RecentPost()
        {
            //Select 5 most recent post and pass to a partialview
            List<Post> recent =
                (from hurr in db.Post
                 orderby hurr.PostedDate descending
                 select hurr).Take(5).ToList();
            ViewBag.Recent = recent;
            return PartialView("~/Views/Shared/Partials/_RecentPost.cshtml");
        }

        [ChildActionOnly]
        public ActionResult TopPosters()
        {
            //Select Users based on number of post made
            List<UserStats> users =
                (from x in db.UserStat
                 orderby x.TotalPost descending
                 select x).Take(5).ToList();
            ViewBag.Top = users;
            return PartialView("~/Views/Shared/Partials/TopPosters.cshtml");
        }
    }
}