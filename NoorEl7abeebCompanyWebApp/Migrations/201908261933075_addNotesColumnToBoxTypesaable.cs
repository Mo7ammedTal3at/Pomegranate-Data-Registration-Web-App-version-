namespace NoorEl7abeebCompanyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotesColumnToBoxTypesaable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Notes", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Notes");
        }
    }
}
