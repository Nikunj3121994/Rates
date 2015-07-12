using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StankinQuestionnaire.Data.Repository
{
    public interface IAcademicRankRepository : IRepository<AcademicRank>
    {
        IEnumerable<AcademicRank> GetRatingsRanksWithGroups();
        AcademicRank GetRatingRankWithGroups(long rankID);
        void UpdateRatings(IEnumerable<RatingGroup> groups, long academicRankID);
        //void Add(AcademicRank academicRank);
        IEnumerable<AcademicRank> GetCatalogAcademicRanks();
        IEnumerable<AcademicRank> GetPossibleParents();
        void UpdateWithParent(AcademicRank entity);
    }

    public class AcademicRankRepository : RepositoryBase<AcademicRank>, IAcademicRankRepository
    {
        public AcademicRankRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<AcademicRank> GetRatingsRanksWithGroups()
        {
            var ranks = DataContext.AcademicRanks.Where(ar => ar.ParentID == null)
                   .Include(ar => ar.RatingGroups).ToList();
            return ranks;
        }


        public AcademicRank GetRatingRankWithGroups(long rankID)
        {
            var rank = DataContext.AcademicRanks.
                Where(ar => ar.AcademicRankID == rankID)
                .Include(ar => ar.RatingGroups)
                .FirstOrDefault();
            return rank;
        }

        public override void Update(AcademicRank entity)
        {
            var academicRank = DataContext.AcademicRanks.Find(entity.AcademicRankID);
            academicRank.Title = entity.Title;
            DataContext.SaveChanges();
        }

        public void UpdateRatings(IEnumerable<RatingGroup> groups, long academicRankID)
        {
            var academicRank = DataContext
                .AcademicRanks
                .Where(ar => ar.AcademicRankID == academicRankID)
                .Include(ar => ar.RatingGroups)
                .FirstOrDefault();
            var ratingsForUpdate = groups.Where(g => g.RatingGroupID != 0);
            var ratingsForAdd = groups.Where(g => g.RatingGroupID == 0);
            var ratingsIdForUpdate = ratingsForUpdate.Select(r => r.RatingGroupID);

            for (int i = 0; i < academicRank.RatingGroups.Count; i++)
            {
                if (!ratingsIdForUpdate.Contains(academicRank.RatingGroups[i].RatingGroupID))//удаление
                {
                    DataContext.RatingGroups.Remove(academicRank.RatingGroups[i]);
                    i--;
                }
            }

            foreach (var group in ratingsForUpdate)//обновление
            {
                var ratingGroup = academicRank.RatingGroups
                      .First(rg => rg.RatingGroupID == group.RatingGroupID);
                ratingGroup.MaxLimit = group.MaxLimit;
                ratingGroup.MinLimit = group.MinLimit;
                ratingGroup.Name = group.Name;
            }

            foreach (var group in ratingsForAdd)//добавление
            {
                academicRank.RatingGroups.Add(new RatingGroup
                {
                    MaxLimit = group.MaxLimit,
                    MinLimit = group.MinLimit,
                    Name = group.Name,
                });
            }
            DataContext.SaveChanges();
        }

        public override void Add(AcademicRank academicRank)
        {
            DataContext.AcademicRanks.Add(academicRank);
            DataContext.SaveChanges();
        }

        public IEnumerable<AcademicRank> GetCatalogAcademicRanks()
        {
            var academicRanks = DataContext.AcademicRanks.Where(ar => ar.ParentID != null)
                .Include(ar => ar.Parent);
            return academicRanks .ToList();
        }

        public IEnumerable<AcademicRank> GetPossibleParents()
        {
            var possibleParents = DataContext.AcademicRanks.Where(ar => ar.ParentID == null);
            return possibleParents.ToList();
        }

        public void UpdateWithParent(AcademicRank entity)
        {
            var academicRank = DataContext.AcademicRanks.Find(entity.AcademicRankID);
            academicRank.Title = entity.Title;
            academicRank.ParentID = entity.ParentID;
            DataContext.SaveChanges();
        }
    }
}
