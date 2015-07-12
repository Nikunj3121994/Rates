namespace StankinQuestionnaire.Data.Migrations
{
    using StankinQuestionnaire.Core.Enums;
    using StankinQuestionnaire.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration :
        DbMigrationsConfiguration<StankinQuestionnaire.Data.StankinQuestionnaireEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StankinQuestionnaire.Data.StankinQuestionnaireEntities context)
        {
            const string adminPassword = "QdASdaF@rQAF!234";

            context.Roles.AddOrUpdate(p => p.Name,
                new CustomRole { Id = 1, Name = RoleEnum.Admin.ToString() },
                new CustomRole { Id = 2, Name = RoleEnum.Reviewer.ToString() });


            var user = new ApplicationUser
            {
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                PasswordHash = Crypto.HashPassword(adminPassword),
                UserName = "Admin",
            };
            context.Users.AddOrUpdate(p => p.UserName, user);
            var customUserRole = context.Set<CustomUserRole>();
            customUserRole.AddOrUpdate(usRo => new
            {
                usRo.RoleId,
                usRo.UserId
            },
                new CustomUserRole { RoleId = 1, UserId = 1 });
        }
    }
}
