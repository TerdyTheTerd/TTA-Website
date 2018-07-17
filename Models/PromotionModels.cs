using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FriendRefer
    {
        public long Id { get; set; }
        public string ApplicationId { get; set; }
        public string ReferApplicationId { get; set; }
        public bool Reffered { get; set; }
    }
}