using StankinQuestionnaire.Model;
using StankinQuestionnaire.Web.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.User.Models
{
    public class DocumentsViewModel
    {
        public DocumentsViewModel(IEnumerable<Document> documents, IEnumerable<DocumentType> documentTypes)
        {
            DocumentsToYear(documents);
            AddEmptyDocumentsForCurrentYear(documentTypes);
        }

        private void DocumentsToYear(IEnumerable<Document> documents)
        {
            Years = new List<Year>();
            foreach (var document in documents)
            {
                var currentYear = Years.FirstOrDefault(y => y.IntYear == document.DateCreated.Year);
                if (currentYear == null)
                {
                    currentYear = new Year
                    {
                        IntYear = document.DateCreated.Year,
                        DocumentLinks = new List<DocumentLink>()
                    };
                    Years.Add(currentYear);
                }
                currentYear.DocumentLinks.Add(new DocumentLink
                {
                    //ID = document.DocumentID,
                    Name = document.DocumentType.Name,
                    DocumentTypeID = document.DocumentType.DocumentTypeID,
                    //Year = document.DateCreated.Year
                });
            }
            if (!Years.Any(y => y.IntYear == DateTime.Now.Year))
            {
                Years.Add(new Year { IntYear = DateTime.Now.Year, DocumentLinks = new List<DocumentLink>() });
            }
        }

        private void AddEmptyDocumentsForCurrentYear(IEnumerable<DocumentType> documentTypes)
        {
            var currentYear = Years.First(y => y.IntYear == DateTime.Now.Year);
            var currentDocumentTypeIds = currentYear.DocumentLinks.Select(dl => dl.DocumentTypeID);
            foreach (var documentType in documentTypes)
            {
                if (!currentDocumentTypeIds.Contains(documentType.DocumentTypeID))
                {
                    currentYear.DocumentLinks.Add(new DocumentLink
                    {
                        DocumentTypeID = documentType.DocumentTypeID,
                        Name = documentType.Name
                    });
                }
            }
        }

        public IList<Year> Years { get; set; }
        public long CreatorID { get; set; }
        //public IEnumerable<DocumentType> AllowDouments { get; set; }
    }
    public class Year
    {
        public int IntYear { get; set; }
        public IList<DocumentLink> DocumentLinks { get; set; }
    }
    public class DocumentLink
    {
        //public int Year { get; set; }
        //public long ID { get; set; }
        public string Name { get; set; }
        public long DocumentTypeID { get; set; }
    }

    public class DocumentConstructor
    {
        public long DocumentID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public EditMode? Mode { get; set; }
        public long CreatorID { get; set; }
    }

    public class CheckedEditModel
    {
        public long DocumentID { get; set; }
        public long IndicatorGroupID { get; set; }
        public bool Checked { get; set; }
    }
}