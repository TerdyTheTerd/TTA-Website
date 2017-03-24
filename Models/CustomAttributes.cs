using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{

    public class ViewCountAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var test = int.Parse(filterContext.HttpContext.Request.Url.Segments[3]) ;
            using (var db = new ApplicationDbContext())
            {
                Post currentPost = db.Post.SingleOrDefault(x => x.Id == test);
                currentPost.Views += 1;
                db.SaveChanges();
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class ProfileViewCountAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //To Do- Check is user has viewed profile previously, for now count every Index request as a view if user is authenticated
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //Increase profile views
                var name = filterContext.HttpContext.Request.Url.Segments[3];
                using (var db = new ApplicationDbContext())
                {
                    var user = db.UserStat.Where(x => x.DisplayName == name).SingleOrDefault();
                    user.ProfileViews += 1;
                    db.SaveChanges();
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class PostPointAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Logic to give user points for posting, check for filters such as x post/day, or type of post made
            string name = filterContext.HttpContext.User.Identity.GetUserId();
            
            using (var db = new ApplicationDbContext())
            {
                UserStats user = db.UserStat.SingleOrDefault(x => x.ApplicationUserId == name);
                user.ProfileExp += 5;
                db.SaveChanges();
            }
                base.OnActionExecuted(filterContext);
        }
    }

    public class PostCountAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Check and make sure the post category isnt blacklisted, if no go ahead and increment users post count, other wise return to action
            base.OnActionExecuted(filterContext);
        }
    }

    //Create various action filters for user leveling, mainly for posting and recieving likes on their post
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null || file.ContentLength > 4 * 1024 * 1024)
            {
                return false;
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;

        }
    }
}