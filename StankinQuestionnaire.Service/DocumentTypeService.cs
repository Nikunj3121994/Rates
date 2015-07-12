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
    public interface IDocumentTypeService
    {
        DocumentType GetDocumentType(long documentTypeID);
        IEnumerable<DocumentType> GetDocumentTypes(Expression<Func<DocumentType, bool>> where = null);
        void CreateDocumentType(DocumentType documentType);
        void EditDocumentType(DocumentType documentType);
        void DeleteDocumentType(DocumentType documentType);
    }

    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IDocumentRepository _documentRepository;

        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository, IUnitOfWork unitOfWork, IDocumentRepository documentRepository)
        {
            this._documentTypeRepository = documentTypeRepository;
            this._unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
        }

        public DocumentType GetDocumentType(long documentTypeID)
        {
            return _documentTypeRepository.GetById(documentTypeID);
        }

        public IEnumerable<DocumentType> GetDocumentTypes(Expression<Func<DocumentType, bool>> where = null)
        {
            if (where == null)
            {
                return _documentTypeRepository.GetMany();
            }
            return _documentTypeRepository.GetMany(where);
        }


        public void CreateDocumentType(DocumentType documentType)
        {
            _documentTypeRepository.Add(documentType);
            SaveDocumentType();
        }

        public void EditDocumentType(DocumentType documentType)
        {
            documentType.DateChanged = DateTime.Now;
            _documentTypeRepository.Update(documentType);
            SaveDocumentType();
        }

        public void DeleteDocumentType(DocumentType documentType)
        {
            for (int i = 0; i < documentType.IndicatorsGroups.Count; i++)
            {
                documentType.IndicatorsGroups.Remove(documentType.IndicatorsGroups[i]);
                i--;
            }
            _documentTypeRepository.Delete(documentType);
            SaveDocumentType();
        }


        private void SaveDocumentType()
        {
            _unitOfWork.Commit();
        }
    }

}
