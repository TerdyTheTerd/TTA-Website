using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {

        public string getId(string name)
        {
            string id = 
                (from x in db.UserStat
                 where x.DisplayName.Equals(name)
                 select x.ApplicationUserId).SingleOrDefault();
            return id;
        }
        [HttpGet]
        public ActionResult Index(string id)
        {

            //UserStats currentUser = db.UserStat.Where(x => x.DisplayName == id).SingleOrDefault();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //Handle Ajax call for additional messages
            //Return list of MessageId's that the user is a part of
            List<long> inbox =
                (from hurr in db.UserList
                 where hurr.UserId.Equals(currentUser.Id)
                 select hurr.MessageId).ToList();
            List<InboxViewModel> model = new List<InboxViewModel>();

            //Iterate over the list and return all messages
            //For each message add a new InboxViewModel to the list model
            List<Message> userMessages = new List<Message>();
            foreach (var hurr in inbox)
            {
                InboxViewModel hurrr = new InboxViewModel();
                hurrr.message =
                    (from x in db.Message
                     where x.Id.Equals(hurr)
                     select x).SingleOrDefault();
                hurrr.SenderProfile =
                    (from x in db.UserStat
                     where x.ApplicationUserId.Equals(hurrr.message.ApplicationUserId)
                     select x.ProfilePicture).SingleOrDefault();
                hurrr.SenderName =
                    (from x in db.UserStat
                     where x.ApplicationUserId.Equals(hurrr.message.ApplicationUserId)
                     select x.DisplayName).SingleOrDefault();
                //userMessages.Add(userMessage);
                model.Add(hurrr);
            }
            //Move from ViewBag to a ViewModel with a List for message objects containing the message info as well as the userstats info for diplaying creator/picture
            ViewBag.Inbox = model; //Query for ten most recent
                                   //Need to return messages from MessageTable that have CurrentUserId as a recipient, return original message plus UserStats Info from most recent response
                                   //MessageViewModel message = new MessageViewModel();
            if(Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Message/Partials/Inbox.cshtml");
            }
            return View("~/Views/Message/Index.cshtml");
        }

        [HttpPost]
        public void Index(MessageViewModel model)
        {

        }

        [HttpGet]
        public ActionResult ViewMessage(int id)
        {
            //Query to get original plus all replies made to a message
            ConversationViewModel model = new ConversationViewModel();
             model.message =
                (from x in db.Message
                 where x.Id.Equals(id)
                 select x).SingleOrDefault();
             
            if (model.message.hasReplies)
            {
                 model.replies =
                    (from x in db.Reply
                     where x.MessageId.Equals(id)
                     orderby x.DateCreated descending
                     select x).ToList();
            }
            return PartialView("~/Views/Message/Partials/ViewMessage.cshtml", model);
        }

        [HttpGet]
        public ActionResult Inbox()
        {
            return PartialView("~/Views/Message/Partials/Inbox.cshtml");
        }

        [HttpGet]
        public ActionResult Compose()
        {
            return PartialView("~/Views/Message/Partials/Compose.cshtml");
        }

        [HttpPost]
        public ActionResult Compose(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Message newMessage = new Message { MessageTitle = model.MessageTitle, MessageBody = model.MessageBody, ApplicationUserId = User.Identity.GetUserId(),
                    CreateDate = DateTime.Now, RecipientList = model.RecipientList};
                //newMessage.MessageTitle = ;
                //newMessage.MessageBody = model.MessageBody;
                //newMessage.ApplicationUserId = User.Identity.GetUserId();
                //newMessage.CreateDate = DateTime.Now;
                //newMessage.RecipientList = model.RecipientList;
                db.Message.Add(newMessage);
                db.SaveChanges();
                //Split Recipient list and add new message entry to recipients table for each
                List<string> userList;
                userList = model.RecipientList.Split(new[] {';'} , StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach(var s in userList)
                {

                    RecipientList recipient = new RecipientList { UserId = getId(s), MessageId = newMessage.Id};
                    db.UserList.Add(recipient);
                }
                RecipientList owner = new RecipientList { UserId = User.Identity.GetUserId(), MessageId = newMessage.Id };
                db.UserList.Add(owner);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Sent()
        {
            return PartialView("~/Views/Message/Partials/Sent.cshtml");
        }

        [HttpGet]
        public ActionResult Drafts()
        {
            return PartialView("~/Views/Message/Partials/Drafts.cshtml");
        }
    }
}