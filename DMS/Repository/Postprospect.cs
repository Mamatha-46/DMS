using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.repository
{
    public class Postprospect : Ipostprospect
    {
        private readonly cyberContext _context;
        public Postprospect(cyberContext Context)
        {
            _context = Context;
        }

        

        public async Task<Prospect> insertprospects(Prospect prospect)
        {
            //if (_context != null)
            //{
            //    var reg = await _context.AddAsync(prospect);
            //    await _context.SaveChangesAsync();
            //    return reg.Entity;
            //}
            //return null;

            _context.Prospect.Add(prospect);
            await _context.SaveChangesAsync();
            return prospect;
        }

        public async Task<List<prospectdetails>> GetDetails()
        {
            var context = new cyberContext();
            var item = (from R in context.Prospect
                        join L in context.Distributor on R.Id equals L.DisId
                        select new prospectdetails
                        {
                            DistributorName = L.DistributorName,
                            Name = R.Name,
                            DistributorCName = L.DistributorCName,
                            DistributorNumber = L.DistributorNumber,
                            Email = R.Email,
                            CountryName = R.CountryName,
                            AddedOn = R.AddedOn
                        });
            return  item.ToList();
        }

        //public prospectdetails getprospectbyid(int id)
        //{
        //    return _context.prospectdetails.FirstOrDefault(c => c.Id == id);
        //}
        //public void Updateprospect(prospectdetails pd)
        //{
        //    _context.Update(pd);
        //    _context.SaveChanges();
        //}

        public IEnumerable<Prospect> GetMyData()
        {
            return _context.Prospect.ToList();
        }

        public async Task<IEnumerable<Countries>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<IEnumerable<States>> GetStates()
        {
            return await _context.States.ToListAsync();
        }

        public async Task<IEnumerable<Cities>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<IEnumerable<Industry>> GetIndustry()
        {
            return await _context.Industry.ToListAsync();
        }
    }

        
 }

