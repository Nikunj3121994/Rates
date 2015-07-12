namespace StankinQuestionnaire.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicRanks",
                c => new
                    {
                        AcademicRankID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        ParentID = c.Long(),
                    })
                .PrimaryKey(t => t.AcademicRankID)
                .ForeignKey("dbo.AcademicRanks", t => t.ParentID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.RatingGroups",
                c => new
                    {
                        RatingGroupID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MaxLimit = c.Int(nullable: false),
                        MinLimit = c.Int(nullable: false),
                        AcademicRankID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RatingGroupID)
                .ForeignKey("dbo.AcademicRanks", t => t.AcademicRankID, cascadeDelete: true)
                .Index(t => t.AcademicRankID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubdivisionID = c.Long(),
                        FirstName = c.String(),
                        SecondName = c.String(),
                        MiddleName = c.String(),
                        AcademicRankID = c.Long(),
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
                .ForeignKey("dbo.AcademicRanks", t => t.AcademicRankID)
                .ForeignKey("dbo.Subdivisions", t => t.SubdivisionID)
                .Index(t => t.SubdivisionID)
                .Index(t => t.AcademicRankID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.IndicatorGroups",
                c => new
                    {
                        IndicatorGroupID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        MaxPoint = c.Int(nullable: false),
                        DocumentTypeID = c.Long(),
                        DateChanged = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IndicatorGroupID)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeID)
                .Index(t => t.DocumentTypeID);
            
            CreateTable(
                "dbo.Checkeds",
                c => new
                    {
                        DocumentID = c.Long(nullable: false),
                        IndicatorGroupID = c.Long(nullable: false),
                        DateChecked = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentID, t.IndicatorGroupID })
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.IndicatorGroups", t => t.IndicatorGroupID, cascadeDelete: true)
                .Index(t => t.DocumentID)
                .Index(t => t.IndicatorGroupID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentID = c.Long(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                        Creator_Id = c.Long(nullable: false),
                        DocumentType_DocumentTypeID = c.Long(),
                    })
                .PrimaryKey(t => t.DocumentID)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_DocumentTypeID)
                .Index(t => t.Creator_Id)
                .Index(t => t.DocumentType_DocumentTypeID);
            
            CreateTable(
                "dbo.Calculations",
                c => new
                    {
                        CalculationID = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        CalculationTypeID = c.Long(),
                        DocumentID = c.Long(nullable: false),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.CalculationID)
                .ForeignKey("dbo.CalculationTypes", t => t.CalculationTypeID)
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.CalculationTypeID)
                .Index(t => t.DocumentID)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.CalculationTypes",
                c => new
                    {
                        CalculationTypeID = c.Long(nullable: false, identity: true),
                        UnitName = c.String(),
                        Point = c.Int(nullable: false),
                        MaxPoint = c.Int(),
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
                        DateChanged = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        IndicatorGroupID = c.Long(),
                    })
                .PrimaryKey(t => t.IndicatorID)
                .ForeignKey("dbo.IndicatorGroups", t => t.IndicatorGroupID)
                .Index(t => t.IndicatorGroupID);
            
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
                "dbo.Subdivisions",
                c => new
                    {
                        SubdivisionID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SubdivisionID);
            
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
                "dbo.DocumentActions",
                c => new
                    {
                        DocumentActionId = c.Long(nullable: false),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.DocumentActionId);
            
            CreateTable(
                "dbo.DocumentLogs",
                c => new
                    {
                        DocumentLogId = c.Long(nullable: false, identity: true),
                        DocumentActionId = c.Long(nullable: false),
                        ActionDate = c.DateTime(nullable: false),
                        NewDescription = c.String(),
                        OldDescription = c.String(),
                        DocumentName = c.String(),
                        DocumentYear = c.Int(nullable: false),
                        DocumentId = c.Long(nullable: false),
                        UserName = c.String(),
                        UserId = c.Long(nullable: false),
                        DocumentCreatorId = c.Long(nullable: false),
                        DocumentCreatorName = c.String(),
                    })
                .PrimaryKey(t => t.DocumentLogId)
                .ForeignKey("dbo.DocumentActions", t => t.DocumentActionId, cascadeDelete: true)
                .Index(t => t.DocumentActionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.IndicatorGroupApplicationUsers",
                c => new
                    {
                        IndicatorGroup_IndicatorGroupID = c.Long(nullable: false),
                        ApplicationUser_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.IndicatorGroup_IndicatorGroupID, t.ApplicationUser_Id })
                .ForeignKey("dbo.IndicatorGroups", t => t.IndicatorGroup_IndicatorGroupID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.IndicatorGroup_IndicatorGroupID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SubdivisionApplicationUsers",
                c => new
                    {
                        Subdivision_SubdivisionID = c.Long(nullable: false),
                        ApplicationUser_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subdivision_SubdivisionID, t.ApplicationUser_Id })
                .ForeignKey("dbo.Subdivisions", t => t.Subdivision_SubdivisionID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Subdivision_SubdivisionID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.DocumentLogs", "DocumentActionId", "dbo.DocumentActions");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SubdivisionID", "dbo.Subdivisions");
            DropForeignKey("dbo.SubdivisionApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubdivisionApplicationUsers", "Subdivision_SubdivisionID", "dbo.Subdivisions");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IndicatorGroupApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IndicatorGroupApplicationUsers", "IndicatorGroup_IndicatorGroupID", "dbo.IndicatorGroups");
            DropForeignKey("dbo.Checkeds", "IndicatorGroupID", "dbo.IndicatorGroups");
            DropForeignKey("dbo.IndicatorGroups", "DocumentTypeID", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "DocumentType_DocumentTypeID", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checkeds", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.Calculations", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Calculations", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.Indicators", "IndicatorGroupID", "dbo.IndicatorGroups");
            DropForeignKey("dbo.CalculationTypes", "IndicatorID", "dbo.Indicators");
            DropForeignKey("dbo.Calculations", "CalculationTypeID", "dbo.CalculationTypes");
            DropForeignKey("dbo.AspNetUsers", "AcademicRankID", "dbo.AcademicRanks");
            DropForeignKey("dbo.RatingGroups", "AcademicRankID", "dbo.AcademicRanks");
            DropForeignKey("dbo.AcademicRanks", "ParentID", "dbo.AcademicRanks");
            DropIndex("dbo.SubdivisionApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SubdivisionApplicationUsers", new[] { "Subdivision_SubdivisionID" });
            DropIndex("dbo.IndicatorGroupApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IndicatorGroupApplicationUsers", new[] { "IndicatorGroup_IndicatorGroupID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.DocumentLogs", new[] { "DocumentActionId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Indicators", new[] { "IndicatorGroupID" });
            DropIndex("dbo.CalculationTypes", new[] { "IndicatorID" });
            DropIndex("dbo.Calculations", new[] { "Owner_Id" });
            DropIndex("dbo.Calculations", new[] { "DocumentID" });
            DropIndex("dbo.Calculations", new[] { "CalculationTypeID" });
            DropIndex("dbo.Documents", new[] { "DocumentType_DocumentTypeID" });
            DropIndex("dbo.Documents", new[] { "Creator_Id" });
            DropIndex("dbo.Checkeds", new[] { "IndicatorGroupID" });
            DropIndex("dbo.Checkeds", new[] { "DocumentID" });
            DropIndex("dbo.IndicatorGroups", new[] { "DocumentTypeID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "AcademicRankID" });
            DropIndex("dbo.AspNetUsers", new[] { "SubdivisionID" });
            DropIndex("dbo.RatingGroups", new[] { "AcademicRankID" });
            DropIndex("dbo.AcademicRanks", new[] { "ParentID" });
            DropTable("dbo.SubdivisionApplicationUsers");
            DropTable("dbo.IndicatorGroupApplicationUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DocumentLogs");
            DropTable("dbo.DocumentActions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Subdivisions");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Indicators");
            DropTable("dbo.CalculationTypes");
            DropTable("dbo.Calculations");
            DropTable("dbo.Documents");
            DropTable("dbo.Checkeds");
            DropTable("dbo.IndicatorGroups");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RatingGroups");
            DropTable("dbo.AcademicRanks");
        }
    }
}
