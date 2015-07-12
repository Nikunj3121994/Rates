using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Data.Models
{
    public class CategoryDTO
    {
        public int Year { get; set; }
        public double Point { get; set; }
        public string Name { get; set; }
    }

    public class CategoryYearsDTO
    {
        public int Year { get; set; }
        public double Point { get; set; }
    }
}
