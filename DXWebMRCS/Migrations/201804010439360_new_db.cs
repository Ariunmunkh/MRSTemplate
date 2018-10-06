namespace DXWebMRCS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        BranchID = c.Int(nullable: false, identity: true),
                        NameMon = c.String(),
                        NameEng = c.String(),
                        Logo = c.String(),
                        Image = c.String(),
                        email = c.String(),
                        phone = c.String(),
                        address = c.String(),
                    })
                .PrimaryKey(t => t.BranchID);
            
            CreateTable(
                "dbo.Elearns",
                c => new
                    {
                        ELID = c.Int(nullable: false, identity: true),
                        LessonName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Image = c.String(nullable: false),
                        LessonBody = c.String(),
                        Time = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ELID);
            
            CreateTable(
                "dbo.eServices",
                c => new
                    {
                        eServiceID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LessonId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        DateTime = c.String(),
                        Elearn_ELID = c.Int(),
                    })
                .PrimaryKey(t => t.eServiceID)
                .ForeignKey("dbo.Elearns", t => t.Elearn_ELID)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Elearn_ELID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        CityTown = c.String(),
                        StateProvince = c.String(),
                        Country = c.String(),
                        Email = c.String(),
                        orderField1 = c.String(),
                        orderField2 = c.String(),
                        orderField3 = c.String(),
                        orderField41 = c.Boolean(nullable: false),
                        orderField42 = c.Boolean(nullable: false),
                        orderField43 = c.Boolean(nullable: false),
                        orderField44 = c.Boolean(nullable: false),
                        orderField45 = c.Boolean(nullable: false),
                        orderField451 = c.String(),
                        orderField5 = c.String(),
                        BirthOfDay = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        UserName = c.String(),
                        AvatarPath = c.String(),
                        BranchId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.FileContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TitleEng = c.String(nullable: false),
                        TitleMon = c.String(nullable: false),
                        DescriptionEng = c.String(nullable: false),
                        DescriptionMon = c.String(nullable: false),
                        Image = c.String(),
                        FilePath = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        GalleryID = c.Int(nullable: false, identity: true),
                        TitleMon = c.String(),
                        TitleEng = c.String(),
                        Image = c.String(),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.GalleryID);
            
            CreateTable(
                "dbo.jobdetails",
                c => new
                    {
                        JDID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        gender = c.String(nullable: false),
                        JobName = c.String(),
                        description = c.String(nullable: false),
                        FilePath = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                        DateEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JDID);
            
            CreateTable(
                "dbo.jobs",
                c => new
                    {
                        JID = c.Int(nullable: false, identity: true),
                        JobName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JID);
            
            CreateTable(
                "dbo.lessonids",
                c => new
                    {
                        LID = c.Int(nullable: false, identity: true),
                        lname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LID);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        QID = c.Int(nullable: false, identity: true),
                        UID = c.Int(nullable: false),
                        LID = c.Int(nullable: false),
                        c1a1 = c.Int(nullable: false),
                        c1a2 = c.Int(nullable: false),
                        c1a3 = c.Int(nullable: false),
                        c1a4 = c.Int(nullable: false),
                        c1a5 = c.Int(nullable: false),
                        c1a6 = c.String(nullable: false),
                        c2a1 = c.Int(nullable: false),
                        c2a2 = c.Int(nullable: false),
                        c2a3 = c.Int(nullable: false),
                        c2a4 = c.Int(nullable: false),
                        c2a5 = c.Int(nullable: false),
                        c3a1 = c.Int(nullable: false),
                        c3a2 = c.Int(nullable: false),
                        c3a3 = c.Int(nullable: false),
                        c3a4 = c.Int(nullable: false),
                        c3a5 = c.Int(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QID)
                .ForeignKey("dbo.lessonids", t => t.LID, cascadeDelete: true)
                .ForeignKey("dbo.userids", t => t.UID, cascadeDelete: true)
                .Index(t => t.LID)
                .Index(t => t.UID);
            
            CreateTable(
                "dbo.userids",
                c => new
                    {
                        UID = c.Int(nullable: false, identity: true),
                        uname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UID);
            
            CreateTable(
                "dbo.Magazines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TitleEng = c.String(nullable: false),
                        TitleMon = c.String(nullable: false),
                        DescriptionEng = c.String(nullable: false),
                        DescriptionMon = c.String(nullable: false),
                        Image = c.String(),
                        FilePath = c.String(),
                        FileName = c.String(),
                        FileExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        MenuID = c.Int(nullable: false, identity: true),
                        NameMon = c.String(),
                        NameEng = c.String(),
                        NavigateUrl = c.String(),
                        MenuType = c.String(),
                        Image = c.String(),
                        ParentId = c.Int(),
                        BranchID = c.Int(),
                        OrderNum = c.Int(),
                    })
                .PrimaryKey(t => t.MenuID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        CID = c.Int(nullable: false, identity: true),
                        TitleMon = c.String(nullable: false),
                        TitleEng = c.String(),
                        BodyMon = c.String(),
                        BodyEng = c.String(),
                        Image = c.String(),
                        ImageMedium = c.String(),
                        MenuID = c.Int(),
                        BranchID = c.Int(),
                        ContentType = c.String(),
                        Date = c.DateTime(nullable: false),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.CID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        TitleMon = c.String(nullable: false),
                        TitleEng = c.String(),
                        BodyMon = c.String(),
                        BodyEng = c.String(),
                        Image = c.String(),
                        ImageMedium = c.String(),
                        BeginDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.SafeUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SafeType = c.String(nullable: false),
                        DepartmentType = c.String(),
                        LastName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        Email = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Country = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        WorkPhone = c.String(),
                        OtherPhone = c.String(),
                        Address = c.String(nullable: false),
                        Address2 = c.String(),
                        StateProvince = c.String(),
                        District = c.String(),
                        NowCountry = c.String(nullable: false),
                        NowAddress = c.String(),
                        HomeAddress2 = c.String(),
                        NowStateProvince = c.String(),
                        NowDistrict = c.String(),
                        SafeMail1 = c.Boolean(nullable: false),
                        SafeMail2 = c.Boolean(nullable: false),
                        SafeMail3 = c.Boolean(nullable: false),
                        SafeMail4 = c.Boolean(nullable: false),
                        SafeMail5 = c.Boolean(nullable: false),
                        SafeMail6 = c.Boolean(nullable: false),
                        SafeMail7 = c.Boolean(nullable: false),
                        SafeMail8 = c.Boolean(nullable: false),
                        SafeMail9 = c.Boolean(nullable: false),
                        SafeMail10 = c.Boolean(nullable: false),
                        SafeMail11 = c.Boolean(nullable: false),
                        SafeMail12 = c.Boolean(nullable: false),
                        SafeMail13 = c.Boolean(nullable: false),
                        SafeMail14 = c.Boolean(nullable: false),
                        OtherNews = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SliderPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MonTitle = c.String(),
                        EngTitle = c.String(),
                        ImagePath = c.String(nullable: false),
                        newsId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscribes",
                c => new
                    {
                        SubID = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SubID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        NameMon = c.String(),
                        NameEng = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.TagDetails",
                c => new
                    {
                        TagDetailID = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        SourceID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagDetailID);
            
            CreateTable(
                "dbo.TrainingRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrainingID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Trainings",
                c => new
                    {
                        TrainingID = c.Int(nullable: false, identity: true),
                        NameMon = c.String(),
                        NameEng = c.String(),
                        ContentMon = c.String(),
                        ContentEng = c.String(),
                        Where = c.String(),
                        When = c.DateTime(nullable: false),
                        Duration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        Status = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TrainingID);
            
            CreateTable(
                "dbo.videos",
                c => new
                    {
                        VID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        Link = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questionnaires", "UID", "dbo.userids");
            DropForeignKey("dbo.Questionnaires", "LID", "dbo.lessonids");
            DropForeignKey("dbo.eServices", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.eServices", "Elearn_ELID", "dbo.Elearns");
            DropIndex("dbo.Questionnaires", new[] { "UID" });
            DropIndex("dbo.Questionnaires", new[] { "LID" });
            DropIndex("dbo.eServices", new[] { "UserId" });
            DropIndex("dbo.eServices", new[] { "Elearn_ELID" });
            DropTable("dbo.videos");
            DropTable("dbo.Trainings");
            DropTable("dbo.TrainingRequests");
            DropTable("dbo.TagDetails");
            DropTable("dbo.Tags");
            DropTable("dbo.Subscribes");
            DropTable("dbo.SliderPhotoes");
            DropTable("dbo.SafeUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.News");
            DropTable("dbo.Menu");
            DropTable("dbo.Magazines");
            DropTable("dbo.userids");
            DropTable("dbo.Questionnaires");
            DropTable("dbo.lessonids");
            DropTable("dbo.jobs");
            DropTable("dbo.jobdetails");
            DropTable("dbo.Galleries");
            DropTable("dbo.FileContents");
            DropTable("dbo.UserProfile");
            DropTable("dbo.eServices");
            DropTable("dbo.Elearns");
            DropTable("dbo.Branches");
        }
    }
}
