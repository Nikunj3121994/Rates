using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using StankinQuestionnaire.Model;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class UserViewModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string SubvisionName { get; set; }
        public string Email { get; set; }
    }

    public class UserEditModel
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long? SubvisionID { get; set; }
        public IEnumerable<SelectListItem> Subdivisions { get; set; }
    }
}