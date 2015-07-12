using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Models
{
    public class SubdivisionWithUsersCount
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int UsersCount { get; set; }
        public long SubdivisionID { get; set; }
    }
}
