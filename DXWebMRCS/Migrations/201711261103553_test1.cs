namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Menu", "BodyMon");
            DropColumn("dbo.Menu", "BodyEng");
            DropColumn("dbo.Menu", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menu", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Menu", "BodyEng", c => c.String());
            AddColumn("dbo.Menu", "BodyMon", c => c.String());
        }
    }
}
