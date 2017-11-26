namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menu", "ParentId", "dbo.Menu");
            DropIndex("dbo.Menu", new[] { "ParentId" });
            DropTable("dbo.Elearns");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Elearns",
                c => new
                    {
                        ELID = c.Int(nullable: false, identity: true),
                        LessonName = c.String(nullable: false),
                        LessonNameEng = c.String(),
                        LessonBody = c.String(),
                    })
                .PrimaryKey(t => t.ELID);
            
            CreateIndex("dbo.Menu", "ParentId");
            AddForeignKey("dbo.Menu", "ParentId", "dbo.Menu", "MenuID");
        }
    }
}
