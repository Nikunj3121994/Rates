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

namespace StankinQuestionnaire.Data.Repository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        IEnumerable<Document> GetManyWithDocumentType(Expression<Func<Document, bool>> where = null);
        Document GetDocumentWithAll(long documentID);
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
                                .Include(doc => doc.DocumentType.IndicatorsGroups
                                    .Select(ig => ig.Indicators
                                        .Select(i => i.CalculationTypes
                                            .Select(ct => ct.Calculations))))
                                .FirstOrDefault();

        }
    }
}
