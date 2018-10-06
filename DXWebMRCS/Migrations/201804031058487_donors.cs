namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donors",
                c => new
                    {
                        DID = c.Int(nullable: false, identity: true),
                        Pnamemon = c.String(),
                        Pnameeng = c.String(),
                        ParagraphMon = c.String(),
                        ParagraphEng = c.String(),
                        PositionMon = c.String(),
                        PositionEng = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.DID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Donors");
        }
    }
}
