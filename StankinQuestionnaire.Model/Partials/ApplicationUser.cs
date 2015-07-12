using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Model;

namespace StankinQuestionnaire.Model
{
    public partial class ApplicationUser
    {
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", SecondName, FirstName, MiddleName);
            }
        }
    }
}
