namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Frind : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        PostedDate = c.DateTime(nullable: false),
                        Author = c.String(),
                        Body = c.String(nullable: false),
                        Category = c.String(),
                        Tag = c.String(),
                        Sticky = c.Boolean(nullable: false),
                        Hot = c.Boolean(nullable: false),
                        HasReplies = c.Boolean(nullable: false),
                        Replies = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Posts");
        }
    }
}
