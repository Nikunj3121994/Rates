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
    public interface IDocumentService
    {
        Document GetDocument(long documentID);
        IEnumerable<Document> GetDocuments(Expression<Func<Document, bool>> where = null);
        IEnumerable<Document> GetDocumentsByUser(long userID);
        Document GetFullDocument(long documentID);
        IEnumerable<DocumentType> GetNotFillDocumentType(long userID, DateTime date);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IUserRepository _userRepository;

        public DocumentService(IDocumentRepository documentRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IDocumentTypeRepository documentTypeRepository)
        {
            this._documentRepository = documentRepository;
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
            _documentTypeRepository = documentTypeRepository;
        }

        public Document GetDocument(long documentID)
        {
            return _documentRepository.GetById(documentID);
        }

        public IEnumerable<Document> GetDocuments(Expression<Func<Document, bool>> where = null)
        {
            return _documentRepository.GetMany(where);
        }

        public IEnumerable<Document> GetDocumentsByUser(long userID)
        {
            var user = _userRepository.GetById(userID);
            return _documentRepository.GetManyWithDocumentType(d => d.Creator != null && d.Creator.Id == user.Id);
        }

        public Document GetFullDocument(long documentID)
        {
            return _documentRepository.GetDocumentWithAll(documentID);
        }

        public IEnumerable<DocumentType> GetNotFillDocumentType(long userID, DateTime date)
        {
            var fillDocumentTypeIds = _documentTypeRepository
                .GetFillDocumentTypes(userID, date)
                .Select(dt => dt.DocumentTypeID);
            var notFillDocTypes = _documentTypeRepository.GetMany(dt => !fillDocumentTypeIds.Contains(dt.DocumentTypeID));
            return notFillDocTypes;
        }

        private void SaveDocument()
        {
            _unitOfWork.Commit();
        }
    }
}
