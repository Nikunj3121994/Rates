using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Areas.Admin.Models
{
    public class ReviewerViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public IList<string> IndicatorGroupName { get; set; }
    }

    public class IndicatorGroups
    {
        public IEnumerable<IndicatorGroupSelect> SelectList { get; set; }
        public long DocumentTypeID { get; set; }
    }

    public class ReviewerEditModel
    {
        public long Id { get; set; }
        //public long ReviewerID { get; set; }
        public IEnumerable<IndicatorGroups> IndicatorGroups { get; set; }
        public IEnumerable<DocumentTypeSelect> DocumentTypes { get; set; }
        public IEnumerable<long> SelectedIndicatorGroupsID { get; set; }
        public string Name { get; set; }

        public IndicatorGroups FindIndicatorGroupByIndicatorGroupID(long indicatorGroupID)
        {
            var indicatorGroup = IndicatorGroups.First(ig => ig.SelectList
                  .Select(sl => sl.IndicatorGroupID)
                  .Contains(indicatorGroupID));
            return indicatorGroup;
        }
    }
}