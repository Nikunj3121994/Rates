using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class DocumentAction
    {
        public long DocumentActionId { get; set; }
        public string Action { get; set; }
        public virtual IList<DocumentLog> DocumentLogs { get; set; }
    }
}
