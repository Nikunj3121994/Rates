using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StankinQuestionnaire.Model;

namespace StankinQuestionnaire.Helper
{
    public static class UserHelper
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<ApplicationUser> users)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var user in users)
            {
                selectList.Add(new SelectListItem
                {
                    Text = user.Email,
                    Value = user.Id.ToString()
                });
            }

            selectList.AddNoOptions();
            return selectList;
        }
    }
}