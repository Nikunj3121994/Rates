using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class CheckedViewModel
    {
        public IEnumerable<GroupDocuments> Groups { get; set; }
    }

    public class GroupDocuments
    {
        public string Name { get; set; }
        public IEnumerable<DocumentElement> Documents { get; set; }
    }

    public class DocumentElement
    {
        public int MaxChecked { get; set; }
        public int CountChecked { get; set; }
        public string Name { get; set; }
        public long DocumentID { get; set; }
        public long DocumentTypeID { get; set; }
        public int Year { get; set; }
        public long CreatorID { get; set; }
    }
}