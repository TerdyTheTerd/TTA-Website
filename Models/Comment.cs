using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Comment
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } //Primary Key

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public DateTime PostedDate { get; set; }
        public string Author { get; set; }
        public int PostId { get; set; }
        public int Points { get; set; }

    }

    public class PostComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } //Primary Key

        public DateTime PostedDate { get; set; }
        public string Author { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public int WallPostId { get; set; }
    }
}