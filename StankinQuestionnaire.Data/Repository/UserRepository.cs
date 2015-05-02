using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Data.Infrastructure;
using StankinQuestionnaire.Model;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity;

namespace StankinQuestionnaire.Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUsersWithSubvision(Expression<Func<ApplicationUser, bool>> predicate = null);
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
    }
}
