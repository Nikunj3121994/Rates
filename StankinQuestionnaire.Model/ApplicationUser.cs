using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StankinQuestionnaire.Model
{
    public partial class ApplicationUser : IdentityUser<long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public virtual IList<Document> Documents { get; set; }
        public long? SubdivisionID { get; set; }
        [ForeignKey("SubdivisionID")]
        public virtual Subdivision Subdivision { get; set; }
        public virtual IList<Subdivision> DirectorSubvisions { get; set; }
        public virtual IList<IndicatorGroup> AllowIndicatorGroups { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public virtual AcademicRank AcademicRank { get; set; }
        public long? AcademicRankID { get; set; }
    }
}