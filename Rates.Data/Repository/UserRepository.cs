using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rates.Data.Infrastructure;
using Rates.Model;

namespace Rates.Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
    }
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository//RepositoryBase<ApplicationUser> реализует IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
