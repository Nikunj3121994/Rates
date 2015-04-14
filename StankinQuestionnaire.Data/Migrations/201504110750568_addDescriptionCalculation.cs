namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescriptionCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calculations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calculations", "Description");
        }
    }
}
