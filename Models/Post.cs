using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Post
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } //Primary Key

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title Must be between 5 and 100 Characters")]
        public string Title { get; set; }

        public DateTime PostedDate { get; set; }
        public string Author { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public string Category { get; set; }
        public string Tag { get; set; }
        public bool Sticky { get; set; }
        public bool Hot { get; set; }
        public bool HasReplies { get; set; }
        public int Replies { get; set; }
        public int Views { get; set; }
        public int Points { get; set; }
    }

    public class WallPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string ownerId { get; set; }
        public string posterName { get; set; }

        public string posterId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string postBody { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimePosted { get; set; }
        public bool HasReplies { get; set; }
    }

    public class PostPoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long postId { get; set; }

        public string ApplicationUserId { get; set; }

        public bool hasVoted { get; set; }
        public string VoteType { get; set; }
    }
}