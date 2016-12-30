using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string MessageBody { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title cannot be shorter than 2 characters or longer than 100.")]
        public string MessageTitle { get; set; }

        public DateTime CreateDate { get; set; }
        public Int16 State { get; set; }

        public virtual ApplicationUser UserId { get; set; }


    }
}