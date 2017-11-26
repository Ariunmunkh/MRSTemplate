namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedmenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menu", "NameMon", c => c.String());
            AddColumn("dbo.Menu", "NameEng", c => c.String());
            AddColumn("dbo.Menu", "BodyMon", c => c.String());
            AddColumn("dbo.Menu", "BodyEng", c => c.String());
            AddColumn("dbo.Menu", "Image", c => c.Binary());
            AddColumn("dbo.Menu", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Menu", "Text");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menu", "Text", c => c.String());
            DropColumn("dbo.Menu", "Date");
            DropColumn("dbo.Menu", "Image");
            DropColumn("dbo.Menu", "BodyEng");
            DropColumn("dbo.Menu", "BodyMon");
            DropColumn("dbo.Menu", "NameEng");
            DropColumn("dbo.Menu", "NameMon");
        }
    }
}
