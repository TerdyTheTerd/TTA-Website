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

        public string ApplicationUserId { get; set; } //FK
        public string RecipientList { get; set; } //String seperated by delmiter list of users, only used for archiving

        [Required]
        [DataType(DataType.MultilineText)]
        public string MessageBody { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title cannot be shorter than 2 characters or longer than 100.")]
        public string MessageTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public bool hasReplies { get; set; } //Used to prevent LINQ query if hasReplies is false
        public virtual ApplicationUser User { get; set; }
    }

    public class RecipientList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long MessageId { get; set; } //FK to Message PK_Id
        public string UserId { get; set; } //FK to UserStats FK_ApplicationUserId
    }

    public class MessageReply
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string MessageId { get; set; } //FK to MessageTable_PK MessageId

        [DataType(DataType.MultilineText)]
        public string ReplyBody { get; set; }

        [DataType(DataType.DateTime)]
        public string DateCreated { get; set; }
    }
}