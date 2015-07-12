using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class DocumentTypeAddModel
    {
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public double Weight { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<long> IndicatorGroupSelect { get; set; }
        public DateTime DateChanged { get { return DateTime.Now; } }
        public DateTime DateCreated { get { return DateTime.Now; } }
    }

    public class DocumentTypeFormModel
    {
        public long DocumentTypeID { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public double Weight { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem> IndicatorGroups { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class DocumentTypeEditModel
    {
        public long DocumentTypeID { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public bool IsActive { get; set; }
        public double Weight { get; set; }
        public IEnumerable<long> IndicatorGroups { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class DocumentTypeViewModel
    {
        public IEnumerable<DocumentTypeFormModel> DocumentTypes { get; set; }
        public DocumentTypeAddModel AddDocumentType { get; set; }
        public IEnumerable<IndicatorGroupSelect> IndicatorGroupSelect { get; set; }
    }

    public class DocumentTypeSelect
    {
        public long DocumentTypeID { get; set; }
        public string Name { get; set; }
    }
}