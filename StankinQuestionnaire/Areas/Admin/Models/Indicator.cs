using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class IndicatorSelect
    {
        public int IndicatorID { get; set; }
        public string Name { get; set; }
    }

    public class IndicatorAddModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<long> CalculationTypeSelect { get; set; }
        public DateTime DateChanged { get { return DateTime.Now; } }
        public DateTime DateCreated { get { return DateTime.Now; } }
    }

    public class IndicatorFormModel
    {
        public long IndicatorID { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<SelectListItem> CalculationTypes { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class IndicatorEditModel
    {
        public long IndicatorID { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<long> CalculationTypes { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class IndicatorViewModel
    {
        public IEnumerable<IndicatorFormModel> Indicators { get; set; }
        public IndicatorAddModel AddIndicator { get; set; }
        public IEnumerable<CalculationTypeSelect> CalculationTypeSelect { get; set; }
    }
}