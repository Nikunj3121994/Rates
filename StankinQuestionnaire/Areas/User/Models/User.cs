using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.User.Models
{
    public class UserRating
    {
        public double Point { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
    }
}