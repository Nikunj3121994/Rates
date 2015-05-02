using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class Subdivision
    {
        public int SubdivisionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IList<ApplicationUser> Employees { get; set; }
        public virtual IList<ApplicationUser> Directors { get; set; }
    }
}
