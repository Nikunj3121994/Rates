using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class RatingGroup
    {
        public long RatingGroupID { get; set; }
        public string Name { get; set; }
        public int MaxLimit { get; set; }
        public int MinLimit { get; set; }
        public virtual AcademicRank AcademicRank { get; set; }
        public long AcademicRankID { get; set; }
    }
}
