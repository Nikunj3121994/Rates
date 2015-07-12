using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StankinQuestionnaire.Data.Repository
{
    public interface ICheckedRepository : IRepository<Checked>
    {
        IEnumerable<DocumentsWithMaxCheckedCountDTO> GetDocuments(long userID);
        IEnumerable<long> GetAllowIndicatorsByDocument(long documentID, long userID);
        void SetCheck(long documentID, long indicatorGroupID, bool checkSate);
    }

    public class CheckRepository : RepositoryBase<Checked>, ICheckedRepository
    {
        public CheckRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<DocumentsWithMaxCheckedCountDTO> GetDocuments(long userID)
        {
            var documents = DataContext.Users.Where(u => u.Id == userID)
             .SelectMany(u => u.AllowIndicatorGroups)
             .Select(aig => aig.DocumentType)
             .GroupBy(dt => dt.DocumentTypeID)
             .Select(g => new DocumentsWithMaxCheckedCountDTO
             {
                 Documents = g.SelectMany(gr => gr.Documents.Select(d => new DocumentsWithCheckedCountDTO
                 {
                     MaxChecked = g.Count(),
                     Document = d,
                     CountChecked = d.Checked.Count(),
                     Name = d.Creator.FirstName + " " + d.Creator.SecondName + " " + d.Creator.MiddleName,
                     DocumentTypeID = d.DocumentType.DocumentTypeID,
                     CreatorID = d.Creator.Id
                 })).Distinct(),
                 Name = g.FirstOrDefault().Name
             })
             .ToList();
            return documents;
        }

        public IEnumerable<long> GetAllowIndicatorsByDocument(long documentID, long userID)
        {
            var documentTypeID = DataContext.Documents.Where(d => d.DocumentID == documentID)
                .Select(d => d.DocumentType.DocumentTypeID)
                .FirstOrDefault();

            var allowIndicatorGroupsID = DataContext.Users.Where(u => u.Id == userID)
                 .SelectMany(u => u.AllowIndicatorGroups.Where(aig => aig.DocumentTypeID == documentTypeID))
                 .Select(ig => ig.IndicatorGroupID);
            return allowIndicatorGroupsID.ToList();
        }

        public void SetCheck(long documentID, long indicatorGroupID, bool checkSate)
        {
            var check = DataContext.Checkeds
                .FirstOrDefault(ch => ch.DocumentID == documentID && ch.IndicatorGroupID == indicatorGroupID);
            if (checkSate)
            {
                if (check == null)
                {
                    var newCheck = new Checked
                    {
                        DateChecked = DateTime.Now,
                        DocumentID = documentID,
                        IndicatorGroupID = indicatorGroupID
                    };
                    DataContext.Checkeds.Add(newCheck);
                }
            }
            else
            {
                if (check != null)
                {
                    DataContext.Checkeds.Remove(check);
                }
            }
            DataContext.SaveChanges();
        }
    }
}
