using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StankinQuestionnaire.Service
{
    public interface ICalculationService
    {
        Calculation GetCalculation(long userId);
        IEnumerable<Calculation> GetCalculations(Expression<Func<Calculation, bool>> where);
        //IEnumerable<Calculation> GetUserCalculationsByCalculationTypes(IEnumerable<long> calculationTypeIDs, long userID);
        void AddCalculation(Calculation calculation);
        bool CalculationAlowUser(long calculationID, long userID);
        void UpdateCalculation(Calculation calculation);
        void DeleteCalculation(long calculationID);
    }

    public class СalculationService : ICalculationService
    {
        private readonly ICalculationRepository _calculationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public СalculationService(ICalculationRepository calculationRepository, IUnitOfWork unitOfWork)
        {
            this._calculationRepository = calculationRepository;
            _unitOfWork = unitOfWork;
        }

        public Calculation GetCalculation(long calculationId)
        {
            return _calculationRepository.GetById(calculationId);
        }

        public IEnumerable<Calculation> GetCalculations(Expression<Func<Calculation, bool>> where)
        {
            return _calculationRepository.GetMany(where);
        }

        //public IEnumerable<Calculation> GetUserCalculationsByCalculationTypes(IEnumerable<long> calculationTypeIDs, long userID)
        //{
        //    return _calculationRepository
        //        .GetMany(c => c.Owner.Id == userID && c.CalculationTypeID.HasValue && calculationTypeIDs.Contains(c.CalculationTypeID.Value));
        //}

        public void AddCalculation(Calculation calculation)
        {
            _calculationRepository.Add(calculation);
            SaveCalculation();
        }

        public void UpdateCalculation(Calculation calculation)
        {
            _calculationRepository.Update(calculation);
            SaveCalculation();
        }

        public bool CalculationAlowUser(long calculationID, long userID)
        {
            return _calculationRepository.IsCalculationAlowUser(calculationID, userID);
            //return _calculationRepository.Any(c => c.Owner.Id == userID && c.CalculationID == calculationID);
        }

        public void DeleteCalculation(long calculationID)
        {
            _calculationRepository.Delete(c => c.CalculationID == calculationID);
            SaveCalculation();
        }

        private void SaveCalculation()
        {
            _unitOfWork.Commit();
        }
    }
}
