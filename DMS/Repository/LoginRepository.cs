using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;
using Microsoft.Data.SqlClient;

namespace DMS.Repository
{
    public class LoginRepository : ILogin
    {
        //cyberContext _dbContext;
        //public LoginRepository(cyberContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public User ValidateUser(string userName, string passWord)
        {
            try
            {
                using (var _context = new cyberContext())
                {
                    var validate = (from user in _context.User
                                    where user.UserMail == userName && user.Pwd == passWord
                                    select user).SingleOrDefault();

                    return validate;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        public async Task<(Distributor,User)> addDistributor(Distributor dist,User use)
        {
            using(var _cyberContext = new cyberContext())
            {
                _cyberContext.Distributor.Add(dist);
                _cyberContext.User.Add(use);
                await _cyberContext.SaveChangesAsync();
                return (dist, use);
            }
            //var _cyberContext = new cyberContext();
            //_cyberContext.Distributor.Add(dist);
            //_cyberContext.User.Add(use);
            //await _cyberContext.SaveChangesAsync();
            //return (dist, use);
        }
    }
}
