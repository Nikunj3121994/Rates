using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Model
{
    public class DocumentLog
    {
        public long DocumentLogId { get; set; }

        public long DocumentActionId { get; set; }
        public virtual DocumentAction DocumentAction { get; set; }

        public DateTime ActionDate { get; set; }

        public string NewDescription { get; set; }
        public string OldDescription { get; set; }

        public string DocumentName { get; set; }
        public int DocumentYear { get; set; }
        public long DocumentId { get; set; }

        public string UserName { get; set; }
        public long UserId { get; set; }

        public long DocumentCreatorId { get; set; }
        public string DocumentCreatorName { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}
