using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.User.Models
{


    public class SaveCalculation
    {
        public long? CalculationID { get; set; }
        public string Description { get; set; }
        public long CalculationTypeID { get; set; }
        public long DocumentID { get; set; }
    }
}