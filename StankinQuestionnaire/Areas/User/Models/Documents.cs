using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.User.Models
{
    public class DocumentsViewModel
    {
        public DocumentsViewModel(IEnumerable<Document> documents)
        {
            DocumentsToYear(documents);
        }

        private void DocumentsToYear(IEnumerable<Document> documents)
        {
            Years = new List<Year>();
            foreach (var document in documents)
            {
                var currentYear = Years.FirstOrDefault(y => y.Date.Year == document.DateCreated.Year);
                if (currentYear == null)
                {
                    currentYear = new Year
                    {
                        Date = document.DateCreated,
                        DocumentLinks = new List<DocumentLink>()
                    };
                    Years.Add(currentYear);
                }
                currentYear.DocumentLinks.Add(new DocumentLink
                {
                    ID = document.DocumentID,
                    Name = document.DocumentType.Name
                });
            }
        }

        public IList<Year> Years { get; set; }
    }
    public class Year
    {
        public DateTime Date { get; set; }
        public IList<DocumentLink> DocumentLinks { get; set; }
    }
    public class DocumentLink
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}