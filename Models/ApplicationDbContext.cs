using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Migrations;

namespace WebApplication1.Models
{
    public interface IApplicationDbContext
    {
        IDbSet<UserStats> UserStat { get; set; }
        IDbSet<Post> Post { get; set; }
        IDbSet<WallPost> WallPost { get; set; }
        IDbSet<Friends> Friend { get; set; }
        IDbSet<PostComment> WallPostReply { get; set; }
        IDbSet<Message> Message { get; set; }
        IDbSet<RecipientList> UserList { get; set; }
        IDbSet<MessageReply> Reply { get; set; }
        IDbSet<PostPoint> PostPoint { get; set; }
        IDbSet<CommentPoint> CommentPoint { get; set; }
        IDbSet<Levels> UserLevel { get; set; }
        IDbSet<WallPostComment> WallPostComment { get; set; }
        IDbSet<UserTags> Tags { get; set; }
        IDbSet<Tick> Tick { get; set; }
        IDbSet<LevelEffects> Effect { get; set; }
        IDbSet<FriendRefer> Referal { get; set; }
        void Dispose();
        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public IDbSet<UserStats> UserStat { get; set; }
        public IDbSet<Post> Post { get; set; }
        public IDbSet<WallPost> WallPost { get; set; }
        public IDbSet<Friends> Friend { get; set; }
        public IDbSet<PostComment> WallPostReply { get; set; }
        public IDbSet<Message> Message { get; set; }
        public IDbSet<RecipientList> UserList { get; set; }
        public IDbSet<MessageReply> Reply { get; set; }
        public IDbSet<PostPoint> PostPoint { get; set; }
        public IDbSet<CommentPoint> CommentPoint { get; set; }
        public IDbSet<Levels> UserLevel { get; set; }
        public IDbSet<WallPostComment> WallPostComment { get; set; }
        public IDbSet<UserTags> Tags { get; set; }
        public IDbSet<Tick> Tick { get; set; }
        public IDbSet<LevelEffects> Effect { get; set; }
        public IDbSet<FriendRefer> Referal { get; set; }
    }

    public class TestApplicationDbContext : IApplicationDbContext
    {
        public IDbSet<UserStats> UserStat { get; set; }
        public IDbSet<Post> Post { get; set; }
        public IDbSet<WallPost> WallPost { get; set; }
        public IDbSet<Friends> Friend { get; set; }
        public IDbSet<PostComment> WallPostReply { get; set; }
        public IDbSet<Message> Message { get; set; }
        public IDbSet<RecipientList> UserList { get; set; }
        public IDbSet<MessageReply> Reply { get; set; }
        public IDbSet<PostPoint> PostPoint { get; set; }
        public IDbSet<CommentPoint> CommentPoint { get; set; }
        public IDbSet<Levels> UserLevel { get; set; }
        public IDbSet<WallPostComment> WallPostComment { get; set; }
        public IDbSet<UserTags> Tags { get; set; }
        public IDbSet<Tick> Tick { get; set; }
        public IDbSet<LevelEffects> Effect { get; set; }
        public IDbSet<FriendRefer> Referal { get; set; }
        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {
            
        }

        
    }
}