using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class IndicatorGroupAddModel
    {
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<long> IndicatorSelect { get; set; }
        public DateTime DateChanged { get { return DateTime.Now; } }
        public DateTime DateCreated { get { return DateTime.Now; } }
    }

    public class IndicatorGroupFormModel
    {
        public long IndicatorGroupID { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<SelectListItem> Indicators { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class IndicatorGroupEditModel
    {
        public long IndicatorGroupID { get; set; }
        [Required]
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<long> Indicators { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class IndicatorGroupViewModel
    {
        public IEnumerable<IndicatorGroupFormModel> IndicatorGroups { get; set; }
        public IndicatorGroupAddModel AddIndicatorGroup { get; set; }
        public IEnumerable<IndicatorSelect> IndicatorSelect { get; set; }
    }

    public class IndicatorGroupSelect
    {
        public long IndicatorGroupID { get; set; }
        public string Name { get; set; }
    }
}