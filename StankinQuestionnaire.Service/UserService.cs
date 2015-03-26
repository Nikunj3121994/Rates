using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Data.Repository;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;

namespace StankinQuestionnaire.Service
{
    public interface IUserService
    {
        ApplicationUser GetUser(long userId);
        IEnumerable<ApplicationUser> GetUsers(Expression<Func<ApplicationUser, bool>> where);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public ApplicationUser GetUser(long userId)
        {
            return userRepository.GetById(userId);
        }

        public IEnumerable<ApplicationUser> GetUsers(Expression<Func<ApplicationUser, bool>> where)
        {
            return userRepository.GetMany(where);
        }
    }
}
