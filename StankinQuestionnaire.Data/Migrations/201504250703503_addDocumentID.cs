namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDocumentID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calculations", "DocumentID", c => c.Long(nullable: false));
            CreateIndex("dbo.Calculations", "DocumentID");
            AddForeignKey("dbo.Calculations", "DocumentID", "dbo.Documents", "DocumentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Calculations", "DocumentID", "dbo.Documents");
            DropIndex("dbo.Calculations", new[] { "DocumentID" });
            DropColumn("dbo.Calculations", "DocumentID");
        }
    }
}
