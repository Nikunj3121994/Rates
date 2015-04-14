namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calculations",
                c => new
                    {
                        CalculationID = c.Long(nullable: false, identity: true),
                        CalculationTypeID = c.Long(),
                        Creator_Id = c.Long(),
                    })
                .PrimaryKey(t => t.CalculationID)
                .ForeignKey("dbo.CalculationTypes", t => t.CalculationTypeID)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id)
                .Index(t => t.CalculationTypeID)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.CalculationTypes",
                c => new
                    {
                        CalculationTypeID = c.Long(nullable: false, identity: true),
                        UnitName = c.String(),
                        Point = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                        IndicatorID = c.Long(),
                    })
                .PrimaryKey(t => t.CalculationTypeID)
                .ForeignKey("dbo.Indicators", t => t.IndicatorID)
                .Index(t => t.IndicatorID);
            
            CreateTable(
                "dbo.Indicators",
                c => new
                    {
                        IndicatorID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MaxPoint = c.Int(nullable: false),
                        Comment = c.String(),
                        DateChanged = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        IndicatorGroupID = c.Long(),
                    })
                .PrimaryKey(t => t.IndicatorID)
                .ForeignKey("dbo.IndicatorGroups", t => t.IndicatorGroupID)
                .Index(t => t.IndicatorGroupID);
            
            CreateTable(
                "dbo.IndicatorGroups",
                c => new
                    {
                        IndicatorGroupID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        DocumentTypeID = c.Long(),
                        DateChanged = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IndicatorGroupID)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeID)
                .Index(t => t.DocumentTypeID);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        DocumentTypeID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MaxPoint = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentTypeID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentID = c.Long(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                        Creator_Id = c.Long(),
                        DocumentType_DocumentTypeID = c.Long(),
                    })
                .PrimaryKey(t => t.DocumentID)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_DocumentTypeID)
                .Index(t => t.Creator_Id)
                .Index(t => t.DocumentType_DocumentTypeID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                        Subdivision_SubdivisionID = c.Int(),
                        Subdivision_SubdivisionID1 = c.Int(),
                        SubdivisionDirector_SubdivisionID = c.Int(),
                        SubdivisionEmployee_SubdivisionID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subdivisions", t => t.Subdivision_SubdivisionID)
                .ForeignKey("dbo.Subdivisions", t => t.Subdivision_SubdivisionID1)
                .ForeignKey("dbo.Subdivisions", t => t.SubdivisionDirector_SubdivisionID)
                .ForeignKey("dbo.Subdivisions", t => t.SubdivisionEmployee_SubdivisionID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Subdivision_SubdivisionID)
                .Index(t => t.Subdivision_SubdivisionID1)
                .Index(t => t.SubdivisionDirector_SubdivisionID)
                .Index(t => t.SubdivisionEmployee_SubdivisionID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
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
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Subdivisions",
                c => new
                    {
                        SubdivisionID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SubdivisionID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Calculations", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Indicators", "IndicatorGroupID", "dbo.IndicatorGroups");
            DropForeignKey("dbo.IndicatorGroups", "DocumentTypeID", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "DocumentType_DocumentTypeID", "dbo.DocumentTypes");
            DropForeignKey("dbo.AspNetUsers", "SubdivisionEmployee_SubdivisionID", "dbo.Subdivisions");
            DropForeignKey("dbo.AspNetUsers", "SubdivisionDirector_SubdivisionID", "dbo.Subdivisions");
            DropForeignKey("dbo.AspNetUsers", "Subdivision_SubdivisionID1", "dbo.Subdivisions");
            DropForeignKey("dbo.AspNetUsers", "Subdivision_SubdivisionID", "dbo.Subdivisions");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CalculationTypes", "IndicatorID", "dbo.Indicators");
            DropForeignKey("dbo.Calculations", "CalculationTypeID", "dbo.CalculationTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "SubdivisionEmployee_SubdivisionID" });
            DropIndex("dbo.AspNetUsers", new[] { "SubdivisionDirector_SubdivisionID" });
            DropIndex("dbo.AspNetUsers", new[] { "Subdivision_SubdivisionID1" });
            DropIndex("dbo.AspNetUsers", new[] { "Subdivision_SubdivisionID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Documents", new[] { "DocumentType_DocumentTypeID" });
            DropIndex("dbo.Documents", new[] { "Creator_Id" });
            DropIndex("dbo.IndicatorGroups", new[] { "DocumentTypeID" });
            DropIndex("dbo.Indicators", new[] { "IndicatorGroupID" });
            DropIndex("dbo.CalculationTypes", new[] { "IndicatorID" });
            DropIndex("dbo.Calculations", new[] { "Creator_Id" });
            DropIndex("dbo.Calculations", new[] { "CalculationTypeID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Subdivisions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Documents");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.IndicatorGroups");
            DropTable("dbo.Indicators");
            DropTable("dbo.CalculationTypes");
            DropTable("dbo.Calculations");
        }
    }
}
