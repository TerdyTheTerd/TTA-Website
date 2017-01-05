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