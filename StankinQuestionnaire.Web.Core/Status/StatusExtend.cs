using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StankinQuestionnaire.Web.Core.Status
{
    public static class StatusExtend
    {
        private const string dictionaryKey = "Status";
        public static void AddStatus(this Controller controller, StatusType statusType, string text, string description = null)
        {
            var status = new Status { Text = text, Description = description, StatusType = statusType };
            controller.TempData[dictionaryKey] = status;
        }

        public static MvcHtmlString GetStatus(this HtmlHelper html)
        {
            var status = html.ViewContext.TempData[dictionaryKey] as Status;
            if (status != null)
            {

                var p = new TagBuilder("p");
                switch (status.StatusType)
                {
                    case StatusType.SUCCESS:
                        p.AddCssClass("bg-success");
                        break;
                    case StatusType.ERROR:
                        p.AddCssClass("bg-danger");
                        break;
                    case StatusType.WARNING:
                        p.AddCssClass("bg-warning");
                        break;
                }
                p.AddCssClass("status");
                p.InnerHtml = string.Format("{0}<br/>{1}", status.Text, status.Description);
                return new MvcHtmlString(p.ToString());
            }
            return null;
        }
    }
}
