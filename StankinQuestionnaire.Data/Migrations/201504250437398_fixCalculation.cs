namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixCalculation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalculationTypes", "MaxPoint", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalculationTypes", "MaxPoint");
        }
    }
}
