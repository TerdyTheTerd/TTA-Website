using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Boolean IsActive { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public string Return_Url { get; set; }
    }
}