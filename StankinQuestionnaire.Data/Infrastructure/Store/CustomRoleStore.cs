using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StankinQuestionnaire.Data;

namespace StankinQuestionnaire.Model.Store
{
    public class CustomRoleStore : RoleStore<CustomRole, long, CustomUserRole>
    {
        public CustomRoleStore(StankinQuestionnaireEntities context)
            : base(context)
        {

        }

    }
}
