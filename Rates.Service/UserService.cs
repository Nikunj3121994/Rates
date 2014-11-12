﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rates.Model;
using Rates.Data.Repository;
using System.Linq.Expressions;

namespace Rates.Service
{
    public interface IUserService
    {
        ApplicationUser GetUser(long userId);
        IEnumerable<ApplicationUser> GetUsers();
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
