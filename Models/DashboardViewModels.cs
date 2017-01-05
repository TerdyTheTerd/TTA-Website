using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class BasicViewModel
    {
        
        [RegularExpression("[a-zA-Z0-9]")]
        public string NickName { get; set; }

        public int Gender { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }

        public string Quote { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }  
    }

    public class UserModel
    {
        public string GetUser(string id)
        {
            var db = new ApplicationDbContext();
            UserStats model = db.UserStat.SingleOrDefault(x => x.ApplicationUserId == id);
            return null;
        }

    }

    public class ImageViewModel
    {
        //Custom Validator, despite returning true, doesnt get applied and the ModelState.IsValid gets turned to false, need to fix for server side validation
        [ValidateFile(ErrorMessage = "Please select a smaller package, we simply can't fit that one inside")]
        public HttpPostedFileBase File { get; set; }
        public int offSetX { get; set; }
        public int offSetY { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string Type { get; set; }
        public string user { get; set; }
    }

    public class AccountBasicViewModel
    {
        public string NickName { get; set; }
        public string Quote { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public string Location { get; set; }
    }
}