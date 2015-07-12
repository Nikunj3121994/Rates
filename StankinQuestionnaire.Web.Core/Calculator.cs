using StankinQuestionnaire.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Web.Core
{
    public interface ICalculator
    {
        double CalculatePointForTeacher(IEnumerable<DocumentJSON> documents);
    }
    public class Calculator:ICalculator
    {
        public double CalculatePointForTeacher(IEnumerable<DocumentJSON> documents)
        {
            double sum = 0;
            foreach (var document in documents)
            {
                sum += CalculatePointForDocument(document) * document.Weight;
            }
            return sum;
        }

        private int CalculatePointForDocument(DocumentJSON document)
        {
            var allPoint = 0;
            foreach (var indicatorGroup in document.IndicatorGroups)
            {
                var indicatorGroupPoint = 0;
                foreach (var indicator in indicatorGroup.Indicators)
                {
                    var indicatorPoint = 0;
                    foreach (var calculationType in indicator.CalculationTypes)
                    {
                        var pointForOneCalculationType = calculationType.Point;
                        var calculationTypePoint = pointForOneCalculationType * calculationType.Calculations.Count;
                        if (calculationType.MaxPoint.HasValue)
                        {
                            if (calculationTypePoint > calculationType.MaxPoint.Value)
                            {
                                calculationTypePoint = calculationType.MaxPoint.Value;
                            }
                        }
                        indicatorPoint += calculationTypePoint;
                    }
                    indicatorGroupPoint += indicatorPoint;
                }
                if (indicatorGroupPoint > indicatorGroup.MaxPoint)
                {
                    indicatorGroupPoint = indicatorGroup.MaxPoint;
                }
                allPoint += indicatorGroupPoint;
            }
            if (allPoint > document.MaxPoint)
            {
                allPoint = document.MaxPoint;
            }
            return allPoint;
        }
    }
}
