using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Rates.Model;
using Rates.Data;

namespace Rates.Data.Infrastructure.Store
{
    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(RatesEntities context)
            : base(context)
        {

        }
    }
}
