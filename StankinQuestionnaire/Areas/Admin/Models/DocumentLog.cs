using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StankinQuestionnaire.Core.Enums;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class DocumentLogViewModel
    {
        public string ActionName { get; set; }
        public DateTime ActionDate { get; set; }
        public string NewDescription { get; set; }
        public string OldDescription { get; set; }
        public string DocumentName { get; set; }
        public int DocumentYear { get; set; }
        public long DocumentId { get; set; }
        public long DocumentCreatorId { get; set; }
        public long DocumentTypeId { get; set; }
        public string DocumentCreatorName { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public DocumentActionEnum Action { get; set; }
    }
}