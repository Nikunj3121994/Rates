using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Model;
using System.Data.Entity;
using System.Linq.Expressions;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Core.Enums;

namespace StankinQuestionnaire.Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUsersWithSubvision(Expression<Func<ApplicationUser, bool>> predicate = null);
        void AddRange(IEnumerable<ApplicationUser> users);
        IEnumerable<ApplicationUser> GetReviewers();
        IEnumerable<long> GetAllowIndicatorGroups(long reviewerID);
        void UpdateAllowIndicatorGroups(long userID, IEnumerable<long> indicatorGroups);
        IEnumerable<ApplicationUser> GetForSubdivision(long subdivisionID);
        IEnumerable<RatingGroup> GetRatingGroups(long userId);
    }
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository//RepositoryBase<ApplicationUser> реализует IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<ApplicationUser> GetUsersWithSubvision(Expression<Func<ApplicationUser, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return DataContext.Users
                       .Where(predicate)
                       .Include(u => u.Subdivision);
            }
            return DataContext.Users
                       .Include(u => u.Subdivision);
        }

        public override void Update(ApplicationUser entity)
        {
            var user = GetById(entity.Id);
            user.Email = entity.Email;
            user.UserName = entity.UserName;
            user.SubdivisionID = entity.SubdivisionID;
            DataContext.SaveChanges();
        }

        public void AddRange(IEnumerable<ApplicationUser> users)
        {
            foreach (var user in users)
            {
                DataContext.Users.Add(user);
            }
            DataContext.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetReviewers()
        {
            var usersID = DataContext
                .Roles
                .Where(r => r.Name == RoleEnum.Reviewer.ToString())
                .SelectMany(r => r.Users)
                .Select(u => u.UserId);
            var users = DataContext
                .Users
                .Where(u => usersID.Contains(u.Id))
                .Include(u => u.AllowIndicatorGroups)
                .ToList();
            return users;
        }

        public IEnumerable<long> GetAllowIndicatorGroups(long reviewerID)
        {
            var indicatorGropusID = DataContext
                  .Users
                  .Where(u => u.Id == reviewerID)
                  .SelectMany(u => u.AllowIndicatorGroups.Select(aig => aig.IndicatorGroupID))
                  .ToList();
            return indicatorGropusID;
        }

        public void UpdateAllowIndicatorGroups(long userID, IEnumerable<long> indicatorGroupsID)
        {
            var user = GetById(userID);
            var indicatorGroups = DataContext.IndicatorGroups
                .Where(ig => indicatorGroupsID.Contains(ig.IndicatorGroupID))
                .ToList();
            var currentAllowIndicatorGropusID = user.AllowIndicatorGroups.Select(ig => ig.IndicatorGroupID);

            for (int i = 0; i < user.AllowIndicatorGroups.Count; i++)
            {
                if (!indicatorGroupsID.Contains(user.AllowIndicatorGroups[i].IndicatorGroupID))
                {
                    user.AllowIndicatorGroups.Remove(user.AllowIndicatorGroups[i]);
                    i--;
                }
            }

            foreach (var allowIndicatorGroupID in indicatorGroupsID)
            {
                if (!currentAllowIndicatorGropusID.Contains(allowIndicatorGroupID))
                {
                    var indicatorGroup = indicatorGroups.First(ig => ig.IndicatorGroupID == allowIndicatorGroupID);
                    user.AllowIndicatorGroups.Add(indicatorGroup);
                }
            }
            DataContext.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetForSubdivision(long subdivisionID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RatingGroup> GetRatingGroups(long userId)
        {
            var academicRanks = DataContext.Users.Where(u => u.Id == userId)
                  .SelectMany(u => u.AcademicRank.RatingGroups);
            return academicRanks.ToList();
        }
    }
}
