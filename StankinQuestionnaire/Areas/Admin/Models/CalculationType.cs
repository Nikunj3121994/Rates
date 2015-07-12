using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class CalculationTypeAddModel
    {
        [Required]
        public string UnitName { get; set; }
        public int Point { get; set; }
        public int? MaxPoint { get; set; }
        public DateTime DateChanged { get { return DateTime.Now; } }
        public DateTime DateCreated { get { return DateTime.Now; } }
    }

    public class CalculationTypeSelect
    {
        public int CalculationTypeID { get; set; }
        public string UnitName { get; set; }
    }

    public class CalculationTypeFormModel
    {
        public long CalculationTypeID { get; set; }
        public string UnitName { get; set; }
        public int Point { get; set; }
        public int? MaxPoint { get; set; }
        public string DateChanged { get; set; }
        public string DateCreated { get; set; }
    }

    public class CalculationTypeViewModel
    {
        public IEnumerable<CalculationTypeFormModel> CalculationTypes { get; set; }
        public CalculationTypeAddModel AddCalculationType { get; set; }
    }
}