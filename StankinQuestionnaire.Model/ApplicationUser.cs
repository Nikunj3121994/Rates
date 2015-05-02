using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StankinQuestionnaire.Model
{
    public class ApplicationUser : IdentityUser<long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public virtual IList<Document> Documents { get; set; }
        public int? SubvisionID { get; set; }
        public virtual Subdivision Subdivision { get; set; }
        public virtual IList<Subdivision> DirectorSubvisions { get; set; }
    }
}