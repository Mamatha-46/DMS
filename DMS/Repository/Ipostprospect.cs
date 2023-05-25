using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;

namespace DMS.repository
{
   public interface Ipostprospect
    {
        Task<Prospect> insertprospects(Prospect prospect);
        Task<List<prospectdetails>> GetDetails();

        //prospectdetails getprospectbyid(int id);
        //void Updateprospect(prospectdetails pd);
        IEnumerable<Prospect> GetMyData();
        Task<IEnumerable<Countries>> GetCountries();
        Task<IEnumerable<States>> GetStates();
        Task<IEnumerable<Cities>> GetCities();
        Task<IEnumerable<User>> GetUser();
        Task<IEnumerable<Industry>> GetIndustry();
    }
}
