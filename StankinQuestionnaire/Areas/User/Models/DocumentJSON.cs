using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.User.Models
{
    public class DocumentJSON
    {
        public long DocumentID { get; set; }
        public IEnumerable<IndicatorGroupJSON> IndicatorGroups { get; set; }
    }
    public class CalculationJSON
    {
        public long CalculationID { get; set; }
        public long CalculationTypeID { get; set; }
        public string Description { get; set; }
    }

    public class CalculationTypeJSON
    {
        public long CalculationTypeID { get; set; }
        public string UnitName { get; set; }
        public int Point { get; set; }
        public IEnumerable<CalculationJSON> Calculations { get; set; }
    }

    public class IndicatorJSON
    {
        public long IndicatorID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<CalculationTypeJSON> CalculationTypes { get; set; }
    }

    public class IndicatorGroupJSON
    {
        public long IndicatorGroupID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<IndicatorJSON> Indicators { get; set; }
    }

    public class SaveCalculation
    {
        public long? CalculationID { get; set; }
        public string Description { get; set; }
        public long CalculationTypeID { get; set; }
        public long DocumentID { get; set; }
    }
}