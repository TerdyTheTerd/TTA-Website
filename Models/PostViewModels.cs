using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PostPreviewViewModel 
    {
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}