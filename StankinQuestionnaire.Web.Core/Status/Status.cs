using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Web.Core.Status
{
    public class Status
    {
        public string Text { get; set; }
        public string Description { get; set; }
        public StatusType StatusType { get; set; }
    }
}
