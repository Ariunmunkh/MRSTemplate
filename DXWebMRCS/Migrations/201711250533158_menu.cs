namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        MenuID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        NavigateUrl = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.MenuID)
                .ForeignKey("dbo.Menu", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menu", "ParentId", "dbo.Menu");
            DropIndex("dbo.Menu", new[] { "ParentId" });
            DropTable("dbo.Menu");
        }
    }
}
