namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcontent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        CID = c.Int(nullable: false, identity: true),
                        TitleMon = c.String(nullable: false),
                        TitleEng = c.String(),
                        BodyMon = c.String(),
                        BodyEng = c.String(),
                        Image = c.Binary(),
                        ContentType = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfile");
            DropTable("dbo.News");
        }
    }
}
