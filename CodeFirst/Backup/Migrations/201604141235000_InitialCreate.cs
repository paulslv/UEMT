namespace CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.NewLists",
                c => new
                    {
                        ListID = c.Int(nullable: false, identity: true),
                        ListName = c.String(),
                        DefaultFromEmail = c.String(),
                        DefaultFromName = c.String(),
                        CompanyorOrganization = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        CountryID = c.Int(),
                        City = c.String(),
                        PhoneNumber = c.String(),
                        CreatedDate = c.DateTime(),
                        LastUpdated = c.DateTime(),
                        ExcelCSVFilePath = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.ListID)
                .ForeignKey("dbo.Countries", t => t.CountryID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.ListSusbscribers",
                c => new
                    {
                        ListSubscribersID = c.Int(nullable: false, identity: true),
                        ListID = c.Int(),
                        SubscribersID = c.Int(),
                        Subscriber_SubscriberID = c.Int(),
                    })
                .PrimaryKey(t => t.ListSubscribersID)
                .ForeignKey("dbo.NewLists", t => t.ListID)
                .ForeignKey("dbo.Subscribers", t => t.Subscriber_SubscriberID)
                .Index(t => t.ListID)
                .Index(t => t.Subscriber_SubscriberID);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        SubscriberID = c.Int(nullable: false, identity: true),
                        ListID = c.Int(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        AlternateEmailAddress = c.String(),
                        Address = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        AddedDate = c.DateTime(),
                        LastChanged = c.DateTime(),
                        Unsubscribe = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SubscriberID)
                .ForeignKey("dbo.NewLists", t => t.ListID)
                .Index(t => t.ListID);
            
            CreateTable(
                "dbo.M_Campaigns",
                c => new
                    {
                        Cid = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CTypeId = c.Int(nullable: false),
                        ListId = c.Int(nullable: false),
                        EmailSubject = c.String(),
                        FromName = c.String(),
                        FromEmail = c.String(),
                        EmailContent = c.String(),
                        StatusId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        M_CampTypes_CTId = c.Int(),
                        S_Status_Statid = c.Int(),
                    })
                .PrimaryKey(t => t.Cid)
                .ForeignKey("dbo.M_CampTypes", t => t.M_CampTypes_CTId)
                .ForeignKey("dbo.NewLists", t => t.ListId, cascadeDelete: true)
                .ForeignKey("dbo.S_Status", t => t.S_Status_Statid)
                .Index(t => t.ListId)
                .Index(t => t.M_CampTypes_CTId)
                .Index(t => t.S_Status_Statid);
            
            CreateTable(
                "dbo.M_CampTypes",
                c => new
                    {
                        CTId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CTId);
            
            CreateTable(
                "dbo.M_UsersListCampaign",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsersID = c.String(maxLength: 128),
                        ListID = c.Int(),
                        CampaignID = c.Int(),
                        M_Campaigns_Cid = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.M_Campaigns", t => t.M_Campaigns_Cid)
                .ForeignKey("dbo.NewLists", t => t.ListID)
                .ForeignKey("dbo.AspNetUsers", t => t.UsersID)
                .Index(t => t.UsersID)
                .Index(t => t.ListID)
                .Index(t => t.M_Campaigns_Cid);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.S_Status",
                c => new
                    {
                        Statid = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Statid);
            
            CreateTable(
                "dbo.M_Tracking",
                c => new
                    {
                        TrackID = c.Int(nullable: false, identity: true),
                        CampId = c.Int(nullable: false),
                        SubsciberId = c.Int(nullable: false),
                        IsOpened = c.DateTime(),
                        M_Campaigns_Cid = c.Int(),
                        Subscriber_SubscriberID = c.Int(),
                    })
                .PrimaryKey(t => t.TrackID)
                .ForeignKey("dbo.M_Campaigns", t => t.M_Campaigns_Cid)
                .ForeignKey("dbo.Subscribers", t => t.Subscriber_SubscriberID)
                .Index(t => t.M_Campaigns_Cid)
                .Index(t => t.Subscriber_SubscriberID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UsersCampaigns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsersID = c.String(maxLength: 128),
                        CampaignID = c.Int(),
                        M_Campaigns_Cid = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.M_Campaigns", t => t.M_Campaigns_Cid)
                .ForeignKey("dbo.AspNetUsers", t => t.UsersID)
                .Index(t => t.UsersID)
                .Index(t => t.M_Campaigns_Cid);
            
            CreateTable(
                "dbo.UsersLists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UsersID = c.String(maxLength: 128),
                        ListID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NewLists", t => t.ListID)
                .ForeignKey("dbo.AspNetUsers", t => t.UsersID)
                .Index(t => t.UsersID)
                .Index(t => t.ListID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersLists", "UsersID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersLists", "ListID", "dbo.NewLists");
            DropForeignKey("dbo.UsersCampaigns", "UsersID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersCampaigns", "M_Campaigns_Cid", "dbo.M_Campaigns");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.M_Tracking", "Subscriber_SubscriberID", "dbo.Subscribers");
            DropForeignKey("dbo.M_Tracking", "M_Campaigns_Cid", "dbo.M_Campaigns");
            DropForeignKey("dbo.M_Campaigns", "S_Status_Statid", "dbo.S_Status");
            DropForeignKey("dbo.M_Campaigns", "ListId", "dbo.NewLists");
            DropForeignKey("dbo.M_UsersListCampaign", "UsersID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.M_UsersListCampaign", "ListID", "dbo.NewLists");
            DropForeignKey("dbo.M_UsersListCampaign", "M_Campaigns_Cid", "dbo.M_Campaigns");
            DropForeignKey("dbo.M_Campaigns", "M_CampTypes_CTId", "dbo.M_CampTypes");
            DropForeignKey("dbo.Subscribers", "ListID", "dbo.NewLists");
            DropForeignKey("dbo.ListSusbscribers", "Subscriber_SubscriberID", "dbo.Subscribers");
            DropForeignKey("dbo.ListSusbscribers", "ListID", "dbo.NewLists");
            DropForeignKey("dbo.NewLists", "CountryID", "dbo.Countries");
            DropIndex("dbo.UsersLists", new[] { "ListID" });
            DropIndex("dbo.UsersLists", new[] { "UsersID" });
            DropIndex("dbo.UsersCampaigns", new[] { "M_Campaigns_Cid" });
            DropIndex("dbo.UsersCampaigns", new[] { "UsersID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.M_Tracking", new[] { "Subscriber_SubscriberID" });
            DropIndex("dbo.M_Tracking", new[] { "M_Campaigns_Cid" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.M_UsersListCampaign", new[] { "M_Campaigns_Cid" });
            DropIndex("dbo.M_UsersListCampaign", new[] { "ListID" });
            DropIndex("dbo.M_UsersListCampaign", new[] { "UsersID" });
            DropIndex("dbo.M_Campaigns", new[] { "S_Status_Statid" });
            DropIndex("dbo.M_Campaigns", new[] { "M_CampTypes_CTId" });
            DropIndex("dbo.M_Campaigns", new[] { "ListId" });
            DropIndex("dbo.Subscribers", new[] { "ListID" });
            DropIndex("dbo.ListSusbscribers", new[] { "Subscriber_SubscriberID" });
            DropIndex("dbo.ListSusbscribers", new[] { "ListID" });
            DropIndex("dbo.NewLists", new[] { "CountryID" });
            DropTable("dbo.UsersLists");
            DropTable("dbo.UsersCampaigns");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.M_Tracking");
            DropTable("dbo.S_Status");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.M_UsersListCampaign");
            DropTable("dbo.M_CampTypes");
            DropTable("dbo.M_Campaigns");
            DropTable("dbo.Subscribers");
            DropTable("dbo.ListSusbscribers");
            DropTable("dbo.NewLists");
            DropTable("dbo.Countries");
        }
    }
}
