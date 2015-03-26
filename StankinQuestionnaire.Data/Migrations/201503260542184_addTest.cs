namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestEntities",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Indicator_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Indicators", t => t.Indicator_ID)
                .Index(t => t.Indicator_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestEntities", "Indicator_ID", "dbo.Indicators");
            DropIndex("dbo.TestEntities", new[] { "Indicator_ID" });
            DropTable("dbo.TestEntities");
        }
    }
}
