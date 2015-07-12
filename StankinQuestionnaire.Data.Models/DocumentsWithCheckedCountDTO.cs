using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Models
{
    public class DocumentsWithCheckedCountDTO
    {
        public Document Document { get; set; }
        public string Name { get; set; }
        public int CountChecked { get; set; }
        public int MaxChecked { get; set; }
        public long DocumentTypeID { get; set; }
        public long CreatorID { get; set; }
    }

    public class DocumentsWithMaxCheckedCountDTO
    {
        public IEnumerable<DocumentsWithCheckedCountDTO> Documents { get; set; }
        public string Name { get; set; }
    }
}
