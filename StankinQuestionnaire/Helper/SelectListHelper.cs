using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StankinQuestionnaire.Helper
{
    public static class SelectListHelper
    {
        public static void AddNoOptions(this IList<SelectListItem> selectList)
        {
            if (selectList.Count == 0)
            {
                var notAllowSelect = new SelectListItem();
                notAllowSelect.Disabled = true;
                notAllowSelect.Selected = true;
                notAllowSelect.Text = "Нет вариантов для выбора";
                selectList.Add(notAllowSelect);
            }
            //else
            //{
            //    var notSelected = new SelectListItem { Disabled = true, Selected = true, Text = "Не выбрано" };
            //    selectList.Add(notSelected);
            //}
        }

        public static void SetSelect(this IEnumerable<SelectListItem> selectList, string value)
        {
            foreach (var select in selectList)
            {
                if (select.Value == value)
                {
                    select.Selected = true;
                }
            }
        }
    }
}