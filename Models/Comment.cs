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
        public long PostId { get; set; }
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

        public int Points { get; set; }
        public long WallPostId { get; set; }
    }
    public class WallPostComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public long WallPostId { get; set; }

    }
    public class CommentPoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long commentId { get; set; }

        public string ApplicationUserId { get; set; }

        public bool hasVoted { get; set; }

        public string VoteType { get; set; }
    }
}