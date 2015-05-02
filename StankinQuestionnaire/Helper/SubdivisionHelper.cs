using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Helper
{
    public static class SubdivisionHelper
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<Subdivision> subdivisions)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var subdivision in subdivisions)
            {
                selectList.Add(new SelectListItem
                {
                    Text = subdivision.Name,
                    Value = subdivision.SubdivisionID.ToString()
                });
            }

            selectList.AddNoOptions();
            return selectList;
        }
    }
}