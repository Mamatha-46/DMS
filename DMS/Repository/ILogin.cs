using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;

namespace DMS.Repository
{
    public interface ILogin
    {
        User ValidateUser(string userName, string passWord);
        Task<(Distributor,User)> addDistributor(Distributor dist,User use);
        //List<Industry> ListofIndustry();
        //List<Countries> ListofCountry();
        //List<States> ListofStates();
        //List<Cities> Listofcities();
    }
}
