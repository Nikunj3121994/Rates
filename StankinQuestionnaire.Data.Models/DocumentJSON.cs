using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Models
{
    public class DocumentJSONDTO
    {
        public long DocumentID { get; set; }
        public string Name { get; set; }
        public IEnumerable<IndicatorGroupForChecker> IndicatorGroups { get; set; }
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
        public int? MaxPoint { get; set; }
        public IList<CalculationJSON> Calculations { get; set; }
    }

    public class IndicatorJSON
    {
        public long IndicatorID { get; set; }
        public string Name { get; set; }
        public IEnumerable<CalculationTypeJSON> CalculationTypes { get; set; }
    }

    public class IndicatorGroupForChecker
    {
        public long IndicatorGroupID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<IndicatorJSON> Indicators { get; set; }
        public bool Checked { get; set; }
    }

    public class DocumentJSON
    {
        public long DocumentID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public double Weight { get; set; }
        public IEnumerable<IndicatorGroupJSON> IndicatorGroups { get; set; }

        public void SetCalculations(IEnumerable<Calculation> calculations)
        {
            foreach (var calculation in calculations)
            {
                if (calculation.CalculationTypeID == null)
                    throw new ArgumentNullException();
                var calculationType = FindCalculationType(calculation.CalculationTypeID.Value);
                if (calculationType != null)
                {
                    if (calculationType.Calculations == null)
                    {
                        calculationType.Calculations = new List<CalculationJSON>();
                    }
                    calculationType.Calculations.Add(new CalculationJSON
                    {
                        CalculationID = calculation.CalculationID,
                        CalculationTypeID = calculation.CalculationTypeID.Value,
                        Description = calculation.Description
                    });
                }
            }
        }

        public void InitCalculations()
        {
            foreach (var indicatorGroup in IndicatorGroups)
            {
                foreach (var indicator in indicatorGroup.Indicators)
                {
                    foreach (var calculationType in indicator.CalculationTypes)
                    {
                        if (calculationType.Calculations == null)
                        {
                            calculationType.Calculations = new List<CalculationJSON>();
                        }
                    }
                }
            }
        }

        private CalculationTypeJSON FindCalculationType(long calculationTypeID)
        {
            foreach (var indicatorGroup in IndicatorGroups)
            {
                foreach (var indicator in indicatorGroup.Indicators)
                {
                    foreach (var calculationType in indicator.CalculationTypes)
                    {
                        if (calculationType.CalculationTypeID == calculationTypeID)
                        {
                            return calculationType;
                        }
                    }
                }
            }
            return null;
        }
    }

    public class IndicatorGroupJSON
    {
        public long IndicatorGroupID { get; set; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public IEnumerable<IndicatorJSON> Indicators { get; set; }
    }
}
