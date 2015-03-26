using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Data;

namespace StankinQuestionnaire.Data.Infrastructure.Store
{
    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(StankinQuestionnaireEntities context)
            : base(context)
        {

        }
    }
}
