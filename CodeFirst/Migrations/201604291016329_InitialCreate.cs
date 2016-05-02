namespace CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //   "dbo.M_MailPlans",
            //   c => new
            //   {
            //       PlanId = c.Int(nullable: false, identity: true),
            //       PlanName = c.String(nullable: false),
            //       MailsAlloted = c.Int(),
            //       IsActive = c.Boolean()
            //   })
            //   .PrimaryKey(t => t.PlanId);

            //AddColumn("dbo.M_MailPlans", "MailsAlloted", c => c.Int(nullable: false));
            //DropColumn("dbo.M_MailPlans", "MailAlloted");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.M_MailPlans", new[] { "PlanId" });
            //DropTable("dbo.M_MailPlans");
            //AddColumn("dbo.M_MailPlans", "MailAlloted", c => c.Int(nullable: false));
            //DropColumn("dbo.M_MailPlans", "MailsAlloted");
        }
    }
}
