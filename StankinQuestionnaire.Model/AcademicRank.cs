using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class AcademicRank
    {
        public long AcademicRankID { get; set; }
        public string Title { get; set; }

        public long? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual AcademicRank Parent { get; set; }

        public virtual IList<ApplicationUser> Users { get; set; }
        public virtual IList<RatingGroup> RatingGroups { get; set; }
    }
}
