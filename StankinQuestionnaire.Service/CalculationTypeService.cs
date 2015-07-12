using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System.Threading.Tasks;
using StankinQuestionnaire.Data.Infrastructure;

namespace StankinQuestionnaire.Service
{
    public interface ICalculationTypeService
    {
        CalculationType GetCalculationType(long calculationTypeID);
        IEnumerable<CalculationType> GetCalculationTypes(Expression<Func<CalculationType, bool>> where = null);
        void CreateCalculationType(CalculationType calculationType);
        void EditCalculationType(CalculationType calculationType);
        void DeleteCalculationType(CalculationType calculationType);
        IEnumerable<CalculationType> GetCalculationsTypeWithIndicator(Expression<Func<CalculationType, bool>> where = null);
        void UpdateIndicator(IEnumerable<long> calculationTypeIDs, long indicatorID);
    }

    public class CalculationTypeService : ICalculationTypeService
    {
        private readonly ICalculationTypeRepository _calculationTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CalculationTypeService(ICalculationTypeRepository calculationTypeRepository, IUnitOfWork unitOfWork)
        {
            this._calculationTypeRepository = calculationTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public CalculationType GetCalculationType(long calculationTypeID)
        {
            return _calculationTypeRepository.GetById(calculationTypeID);
        }

        public IEnumerable<CalculationType> GetCalculationsTypeWithIndicator(Expression<Func<CalculationType, bool>> where = null)
        {
            return _calculationTypeRepository.GetManyWithIndicator(where);
        }

        public IEnumerable<CalculationType> GetCalculationTypes(Expression<Func<CalculationType, bool>> where = null)
        {
            return _calculationTypeRepository.GetMany(where);
        }

        public void CreateCalculationType(CalculationType calculationType)
        {
            _calculationTypeRepository.Add(calculationType);
            SaveCalculationType();
        }

        public void EditCalculationType(CalculationType calculationType)
        {
            calculationType.DateChanged = DateTime.Now;
            _calculationTypeRepository.Update(calculationType);
            SaveCalculationType();
        }

        public void DeleteCalculationType(CalculationType calculationType)
        {
            _calculationTypeRepository.Delete(calculationType);
            SaveCalculationType();
        }

        public void UpdateIndicator(IEnumerable<long> calculationTypeIDs, long indicatorID)
        {
            _calculationTypeRepository.UpdateIndicator(calculationTypeIDs, indicatorID);
            SaveCalculationType();
        }


        private void SaveCalculationType()
        {
            _unitOfWork.Commit();
        }
    }
}
