using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Model;
using System.Linq.Expressions;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Data.Infrastructure;

namespace StankinQuestionnaire.Service
{
    public interface IIndicatorGroupService
    {
        IEnumerable<IndicatorGroup> GetIndicatorGroups(Expression<Func<IndicatorGroup, bool>> where = null);
        IndicatorGroup GetIndicatorGroup(long indicatorGroupID);
        void CreateIndicatorGroup(IndicatorGroup indicatorGroup);
        void EditIndicatorGroup(IndicatorGroup indicatorGroup);
        void DeleteIndicatorGroup(IndicatorGroup indicatorGroup);
        IEnumerable<IndicatorGroup> GetIndicatorGroupWithDocumentType(Expression<Func<IndicatorGroup, bool>> where = null);
        void UpdateDocumentTypes(IEnumerable<long> indicatorGroupIds, long documentTypeID);
    }

    public class IndicatorGroupService : IIndicatorGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIndicatorGroupRepository _indicatorGroupRepository;
        public IndicatorGroupService(IIndicatorGroupRepository indicatorGroupRepository, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._indicatorGroupRepository = indicatorGroupRepository;
        }

        public IEnumerable<IndicatorGroup> GetIndicatorGroups(Expression<Func<IndicatorGroup, bool>> where = null)
        {
            return _indicatorGroupRepository.GetMany(where);
        }

        public IndicatorGroup GetIndicatorGroup(long indicatorGroupID)
        {
            return _indicatorGroupRepository.GetById(indicatorGroupID);
        }

        public void CreateIndicatorGroup(IndicatorGroup indicatorGroup)
        {
            _indicatorGroupRepository.Add(indicatorGroup);
            SaveIndicator();
        }

        public void EditIndicatorGroup(IndicatorGroup indicatorGroup)
        {
            indicatorGroup.DateChanged = DateTime.Now;
            _indicatorGroupRepository.Update(indicatorGroup);
            SaveIndicator();
        }

        public void DeleteIndicatorGroup(IndicatorGroup indicatorGroup)
        {
            for (int i = 0; i < indicatorGroup.Indicators.Count; i++)
            {
                indicatorGroup.Indicators.Remove(indicatorGroup.Indicators[i]);
                i--;
            }
            _indicatorGroupRepository.Delete(indicatorGroup);
            SaveIndicator();
        }

        public void UpdateDocumentTypes(IEnumerable<long> indicatorGroupIds, long documentTypeID)
        {
            _indicatorGroupRepository.UpdateDocumentType(indicatorGroupIds, documentTypeID);
            SaveIndicator();
        }

        public IEnumerable<IndicatorGroup> GetIndicatorGroupWithDocumentType(Expression<Func<IndicatorGroup, bool>> where = null)
        {
            return _indicatorGroupRepository.GetManyWithDocumentType(where);
        }


        private void SaveIndicator()
        {
            _unitOfWork.Commit();
        }
    }
}
