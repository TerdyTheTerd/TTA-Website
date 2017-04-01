using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserInfoViewModel //Slightly more Complex model for user dashboards, pass in extra information to avoid extra calls to server for information that is needed anyways
    {
        public UserStats User { get; set; }
        public UserTags UserTag { get; set; }
        public Levels UserLevel { get; set; }
        public Int32 NumFriends { get; set; }
    }
    public class PostPreviewViewModel
    {
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
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
        [RegularExpression("[a-zA-Z0-9]")]
        public string NickName { get; set; }

        public string Quote { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public string Location { get; set; }
    }

    public class MessageViewModel
    {
        [Required]
        public string MessageTitle { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string MessageBody { get; set; }

        [Required]
        public string RecipientList { get; set; }
    }

    public class InboxViewModel
    {
        public Message message { get; set; }
        public string SenderProfile { get; set; }
        public string SenderName { get; set; }
    }

    public class TestViewModel
    {
        public List<InboxViewModel> userMessages { get; set; }
        public Message recentMessage { get; set; }
    }

    public class ConversationViewModel
    {
        public List<MessageReply> replies { get; set; }
        public Message message { get; set; }
        //public Message reply { get; set; }
    }
    public class PictureViewModel
    {
        [ValidateFile(ErrorMessage = "Please select a smaller package, we simply can't fit that one inside")]
        public HttpPostedFileBase File { get; set; }
        public string user { get; set; }
    }
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public string Category { get; set; }
    }
    public class CommentViewModel
    {
        [Required]
        public string Body { get; set; }
        public long PostId { get; set; }
    }
    public class ThreadReplyViewModel
    {
        public PostComment post { get; set; }
        public UserStats user { get; set; }
        public CommentPoint points { get; set; }
    }
    public class ThreadViewModel
    {
        public Post post { get; set; }
        public UserStats user { get; set; }
        public PostPoint point { get; set; }
    }
    public class LevelViewModel
    {
        public string LevelName { get; set; }
        public int LevelExp { get; set; }
        public string LevelEffects { get; set; }
    }
    public class InfoViewModel
    {
        public string Qoute { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
        public int Post { get; set; }
        public int Points { get; set; }
        public int Xp { get; set; }
        public string ForumGroup { get; set; }
        public int ProfileViews { get; set; }
    }
    public class BasicUserViewModel
    {
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string Effects { get; set; }
        public string Tags { get; set; }
    }
    public class BasicReplyViewModel
    {
        public string Author { get; set; }
        public string Owner { get; set; }
        public string Body { get; set; }
        public DateTime TimePosted { get; set; }
        public string Profile { get; set; }
    }
    public class WallPostViewModel
    {
        public WallPost post { get; set; }
        public BasicUserViewModel user { get; set; }
        public List<BasicReplyViewModel> replies { get; set; }
    }
    public class CreateWebsiteTag
    {
        public string Name { get; set; }
        public string Effect { get; set; }
    }
    public class CreateLevelViewModel
    {
        public string Name { get; set; }
        public int Exp { get; set; }
        public string Effects { get; set; }
    }

    public class UpdateLevelViewModel
    {
        public string Name { get; set; }
        public int Exp { get; set; }
        public string Effects { get; set; }
        public string Color { get; set; }
        public int LevelExp { get; set; }
        public int PrevExp { get; set; }
    }
    public class ReferVIewModel
    {
        public string CurrentUser { get; set; }
        public string FriendName { get; set; }
    }
    public class OrderedUsers
    {
        public string UserTag { get; set; }
        public List<UserStats> user { get; set; }
    }
    public class ImageBlob
    {
        public string ImageUrl { get; set; }
        public int dimX { get; set; }
        public int dimY { get; set; }
        public DateTime UploadedDate { get; set; }
    }
    public class UserImage
    {
        public string Id { get; set; }
        public List<ImageBlob> ImageList {get;set;}
    }
}