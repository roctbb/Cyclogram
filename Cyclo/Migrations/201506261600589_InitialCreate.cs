namespace Cyclo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        startDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                        description = c.String(),
                        name = c.String(),
                        subCategory_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubCategories", t => t.subCategory_ID)
                .Index(t => t.subCategory_ID);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.parent_ID)
                .Index(t => t.parent_ID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        report = c.String(),
                        userID = c.Int(nullable: false),
                        inEvent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Events", t => t.inEvent_ID)
                .Index(t => t.inEvent_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "inEvent_ID", "dbo.Events");
            DropForeignKey("dbo.Events", "subCategory_ID", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "parent_ID", "dbo.Categories");
            DropIndex("dbo.Jobs", new[] { "inEvent_ID" });
            DropIndex("dbo.SubCategories", new[] { "parent_ID" });
            DropIndex("dbo.Events", new[] { "subCategory_ID" });
            DropTable("dbo.Jobs");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Events");
            DropTable("dbo.Categories");
        }
    }
}
