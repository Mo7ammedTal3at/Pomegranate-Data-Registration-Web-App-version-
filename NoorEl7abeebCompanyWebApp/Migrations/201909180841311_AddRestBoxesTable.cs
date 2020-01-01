namespace NoorEl7abeebCompanyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRestBoxesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestBoxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        BoxTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoxTypes", t => t.BoxTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.BoxTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RestBoxes", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.RestBoxes", "BoxTypeId", "dbo.BoxTypes");
            DropIndex("dbo.RestBoxes", new[] { "BoxTypeId" });
            DropIndex("dbo.RestBoxes", new[] { "CustomerId" });
            DropTable("dbo.RestBoxes");
        }
    }
}
