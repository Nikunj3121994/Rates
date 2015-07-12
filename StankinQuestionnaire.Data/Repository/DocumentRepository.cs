using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Web.Core.Enums;
using StankinQuestionnaire.Core.Enums;

namespace StankinQuestionnaire.Data.Repository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetManyWithDocumentType(Expression<Func<Document, bool>> where = null);
        Document GetDocumentWithAll(long documentID);
        DocumentType GetDocumentType(long documentID);
        IEnumerable<Document> GetOld(long userID);
        Document GetByYear(long documentTypeID, long userID, int year);
        DocumentJSONDTO GetDocumentWithAllForChecked(long documentID, long checkerID);
        bool IsOwner(long documentId, long userId);
        IEnumerable<IGrouping<int, Document>> GetGroupByYear(long userId);
        bool IsDocumentAllowUser(EditMode? mode, long documentId, long userId);
    }

    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Document> GetManyWithDocumentType(Expression<Func<Document, bool>> where = null)
        {
            if (DataContext.Documents.Count() == 0)
            {
                return new List<Document>();
            }
            if (where == null)
            {
                return DataContext.Documents.Include(ct => ct.DocumentType).ToList();
            }
            return DataContext.Documents.Where(where).Include(ct => ct.DocumentType).ToList();//
        }

        public Document GetDocumentWithAll(long documentID)
        {
            return DataContext.Documents
                                .Where(d => d.DocumentID == documentID)
                                .Include(d => d.Calculations)
                                .Include(d => d.DocumentType)
                                .Include(doc => doc.DocumentType.IndicatorsGroups
                                    .Select(ig => ig.Indicators
                                        .Select(i => i.CalculationTypes)))
                                .FirstOrDefault();

        }

        public DocumentJSONDTO GetDocumentWithAllForChecked(long documentID, long checkerID)
        {
            var allowIndicatorGroups = DataContext.Users
                .Where(u => u.Id == checkerID)
                .SelectMany(u => u.AllowIndicatorGroups);

            var checkedindicatorGroups = DataContext.Documents
                .Where(d => d.DocumentID == documentID)
                .SelectMany(d => d.Checked)
                .Select(ch => ch.IndicatorGroup);

            var documentDTO = DataContext.Documents
                .Where(d => d.DocumentID == documentID)
                .Select(d => new DocumentJSONDTO
                {
                    DocumentID = d.DocumentID,
                    Name = d.DocumentType.Name,
                    IndicatorGroups = allowIndicatorGroups
                        .Where(aig => aig.DocumentTypeID == d.DocumentType.DocumentTypeID).Select(aig => new IndicatorGroupForChecker
                        {
                            IndicatorGroupID = aig.IndicatorGroupID,
                            MaxPoint = aig.MaxPoint,
                            Name = aig.Name,
                            Checked = checkedindicatorGroups.Contains(aig),
                            Indicators = aig.Indicators.Select(i => new IndicatorJSON
                            {
                                Name = i.Name,
                                IndicatorID = i.IndicatorID,
                                CalculationTypes = i.CalculationTypes.Select(ct => new CalculationTypeJSON
                                {
                                    UnitName = ct.UnitName,
                                    Point = ct.Point,
                                    MaxPoint = ct.MaxPoint,
                                    CalculationTypeID = ct.CalculationTypeID,
                                    Calculations = ct.Calculations.Where(c => c.DocumentID == documentID).Select(c => new CalculationJSON
                                    {
                                        CalculationID = c.CalculationID,
                                        CalculationTypeID = ct.CalculationTypeID,
                                        Description = c.Description
                                    }).ToList()
                                })
                            })
                        })
                })
                .FirstOrDefault();
            return documentDTO;
        }

        public DocumentType GetDocumentType(long documentID)
        {
            var documentType = DataContext
                  .Documents
                  .Where(d => d.DocumentID == documentID)
                  .Select(d => d.DocumentType)
                  .FirstOrDefault();
            return documentType;
        }

        public IEnumerable<Document> GetOld(long userID)
        {
            var now = DateTime.Now;
            return DataContext.Documents
                .Where(d => d.DateCreated.Year != now.Year && d.Creator.Id == userID)
                .Include(d => d.DocumentType);
        }

        public Document GetByYear(long documentTypeID, long userID, int year)
        {
            var document = DataContext.Documents
                .Where(d => d.Creator.Id == userID
                    && d.DocumentType.DocumentTypeID == documentTypeID
                    && d.DateCreated.Year == year)
                    .Include(d => d.DocumentType)
                    .FirstOrDefault();
            return document;
        }

        public bool IsOwner(long documentId, long userId)
        {
            return DataContext
                .Documents
                .Any(d => d.DocumentID == documentId && d.Creator.Id == userId);
        }

        public IEnumerable<IGrouping<int, Document>> GetGroupByYear(long userId)
        {
            var documents = DataContext.Documents
                    .Where(d => d.Creator.Id == userId)
                    .Include(d => d.Calculations)
                    .Include(d => d.DocumentType)
                    .Include(doc => doc.DocumentType.IndicatorsGroups
                        .Select(ig => ig.Indicators
                            .Select(i => i.CalculationTypes)))
                    .GroupBy(d => d.DateCreated.Year);

            return documents.ToList();
        }

        public bool IsDocumentAllowUser(EditMode? mode, long documentId, long userId )
        {
            var adminRoleId = (long)RoleEnum.Admin;
            var checkerRoleId = (long)RoleEnum.Reviewer;
            var user = DataContext.Users.Where(u => u.Id == userId);

            var document = DataContext.Documents.Where(d => d.DocumentID == documentId);
            Expression<Func<Document, bool>> adminPreidcate = d => user.Any(u => u.Roles.Select(r => r.RoleId).Contains(adminRoleId));
            Expression<Func<Document, bool>> checkerPredicate = d => user.Any(u => u.Roles.Select(r => r.RoleId).Contains(checkerRoleId));
            Expression<Func<Document, bool>> headPreidcate = d => user.Any(u => u.DirectorSubvisions.Select(ds => ds.SubdivisionID).Contains(d.Creator.SubdivisionID.Value));
            switch (mode)
            {
                case EditMode.Admin:
                    return document.Any(adminPreidcate);
                case EditMode.Checker:
                    return document.Any(checkerPredicate);
                case EditMode.Head:
                    return document.Any(headPreidcate);
                case EditMode.ReadOnly:
                    return document.Any(adminPreidcate) || document.Any(checkerPredicate) || document.Any(headPreidcate);
                default:
                    return document.Any(d => d.Creator.Id == userId);
            }
         
        }
    }
}
