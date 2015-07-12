using StankinQuestionnaire.Core.Enums;
using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StankinQuestionnaire.Data.Models;

namespace StankinQuestionnaire.Data.Repository
{
    public interface IDocumentLogRepository : IRepository<DocumentLog>
    {
        void AddAdded(long userId, long calculationId);
        void AddUpdated(long userId, long calculationId, string oldDescription);
        void AddDeleted(long userId, long documentId, string description);
        IEnumerable<DocumentLogDTO> GetLogs(System.Linq.Expressions.Expression<Func<DocumentLogDTO, bool>> where = null);
    }

    public class DocumentLogRepository : RepositoryBase<DocumentLog>, IDocumentLogRepository
    {
        public DocumentLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<DocumentLogDTO> GetLogs(System.Linq.Expressions.Expression<Func<DocumentLogDTO, bool>> where = null)
        {
            //var documentType=DataContext.Documents.Where(d=>d.)
            return DataContext.DocumentLogs
                .Select(dl => new DocumentLogDTO
                {
                    ActionDate = dl.ActionDate,
                    DocumentAction = dl.DocumentAction,
                    DocumentActionId = dl.DocumentActionId,
                    DocumentCreatorId = dl.DocumentCreatorId,
                    DocumentCreatorName = dl.DocumentCreatorName,
                    DocumentId = dl.DocumentId,
                    DocumentLogId = dl.DocumentLogId,
                    DocumentName = dl.DocumentName,
                    DocumentYear = dl.DocumentYear,
                    NewDescription = dl.NewDescription,
                    OldDescription = dl.OldDescription,
                    UserId = dl.UserId,
                    UserName = dl.UserName,
                    DocumentTypeId = DataContext.Documents.FirstOrDefault(d => d.DocumentID == dl.DocumentId) != null ? DataContext.Documents.FirstOrDefault(d => d.DocumentID == dl.DocumentId).DocumentType.DocumentTypeID : 0
                })
                .ToList();
        }

        public void AddAdded(long userId, long calculationId)
        {
            var user = DataContext.Users.Find(userId);
            var calculation = DataContext.Calculations
                .Where(c => c.CalculationID == calculationId)
                .Include(c => c.Document)
                .Include(c => c.Document.DocumentType)
                 .Include(c => c.Document.Creator)
                .FirstOrDefault();
            var creator = calculation.Document.Creator;

            DataContext.DocumentLogs.Add(new DocumentLog
            {
                ActionDate = DateTime.Now,
                DocumentActionId = (long)DocumentActionEnum.Add,
                DocumentCreatorId = creator.Id,
                DocumentCreatorName = creator.FullName,
                DocumentId = calculation.DocumentID,
                DocumentName = calculation.Document.DocumentType.Name,
                DocumentYear = calculation.Document.DateCreated.Year,
                NewDescription = calculation.Description,
                UserId = userId,
                UserName = user.FullName
            });
            DataContext.SaveChanges();
        }

        public void AddUpdated(long userId, long calculationId, string oldDescription)
        {
            var user = DataContext.Users.Find(userId);
            var calculation = DataContext.Calculations
                .Where(c => c.CalculationID == calculationId)
                .Include(c => c.Document)
                .Include(c => c.Document.DocumentType)
                .Include(c => c.Document.Creator)
                .FirstOrDefault();
            var creator = calculation.Document.Creator;

            DataContext.DocumentLogs.Add(new DocumentLog
                {
                    ActionDate = DateTime.Now,
                    DocumentActionId = (long)DocumentActionEnum.Update,
                    DocumentCreatorId = creator.Id,
                    DocumentCreatorName = creator.FullName,
                    DocumentId = calculation.DocumentID,
                    DocumentName = calculation.Document.DocumentType.Name,
                    DocumentYear = calculation.Document.DateCreated.Year,
                    NewDescription = calculation.Description,
                    OldDescription = oldDescription,
                    UserId = userId,
                    UserName = user.FullName
                });
            DataContext.SaveChanges();
        }

        public void AddDeleted(long userId, long documentId, string description)
        {
            var document = DataContext.
                Documents
                .Where(d => d.DocumentID == documentId)
                .Include(d => d.DocumentType)
                .Include(d => d.Creator)
                .FirstOrDefault();
            var creator = document.Creator;

            var user = DataContext.Users.Find(userId);

            DataContext.DocumentLogs.Add(new DocumentLog
            {
                ActionDate = DateTime.Now,
                DocumentActionId = (long)DocumentActionEnum.Delete,
                DocumentId = documentId,
                DocumentName = document.DocumentType.Name,
                DocumentYear = document.DateCreated.Year,
                NewDescription = description,
                UserId = userId,
                UserName = user.FullName,
                DocumentCreatorId = creator.Id,
                DocumentCreatorName = creator.FullName
            });
            DataContext
                .SaveChanges();
        }
    }
}
