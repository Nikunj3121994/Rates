using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class Checked
    {
        [Key, Column(Order = 1)]
        public long DocumentID { get; set; }
        public virtual Document Document { get; set; }

        [Key, Column(Order = 2)]
        public long IndicatorGroupID { get; set; }
        public virtual IndicatorGroup IndicatorGroup { get; set; }

        //public long UserID { get; set; }
        //public virtual ApplicationUser User { get; set; }

        public DateTime DateChecked { get; set; }
    }
}
