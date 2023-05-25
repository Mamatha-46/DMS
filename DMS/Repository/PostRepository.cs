using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly cyberContext _context;
        public PostRepository(cyberContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllusers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserById(int UserId)
        {
            return await _context.User.FindAsync(UserId);
        }

        public async Task UpdateUser(User user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<Brand> Createcompany(Brand brand)
        {
            if (_context != null)
            {
                _context.Brand.Add(brand);
                await _context.SaveChangesAsync();
                return brand;

            }
            return null;
        }

        public async Task UpdateCompany(Brand brand)
        {
            try
            {
                _context.Entry(brand).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating brand", ex);
            }

        }
        public async Task<IEnumerable<prospectviewmodel>> GetProspectDetails()
        {
            var prospects = from b in _context.Prospect
                            join c in _context.Countries on b.CountryId equals c.Id into bc
                            from c in bc.DefaultIfEmpty()
                            join d in _context.User on b.AddedId equals d.UserId into bd
                            from d in bd.DefaultIfEmpty()

                            select new prospectviewmodel
                            {
                                Id = b.Id,
                                Name = b.Name,
                                Email = b.Email,
                                Number = b.Number,
                                CPerson = b.CPerson,
                                // industryId = p.industryId,
                                //IndustryType = p.IndustryType,
                                CountryName = c.Name,
                                //CountryId=c.Id,
                                UserFname = d.UserFname,
                                //AddedId=d.UserFname,
                                // AddedBy = b.AddedBy,
                                //UpdatedBy = b.UpdatedBy,
                                //UpdatedId = b.UpdatedId,
                                //UpdatedOn = b.UpdatedOn,

                            };


            return await prospects.ToListAsync();


        }
        public async Task<IEnumerable<brandviewmodel>> GetAllCompanies()
        {
            var query = from b in _context.Brand
                        join c in _context.Countries on b.Country equals c.Id into bc

                        from c in bc.DefaultIfEmpty()
                        join s in _context.States on b.State equals s.Id into bs
                        from s in bs.DefaultIfEmpty()
                        join ct in _context.Cities on b.City equals ct.Id into bct
                        from ct in bct.DefaultIfEmpty()
                        join i in _context.Industry on b.BrandType equals i.Id.ToString() into bi
                        from i in bi.DefaultIfEmpty()


                        select new brandviewmodel
                        {
                            BrandId = b.BrandId,
                            BrandName = b.BrandName,
                            BrandType = b.BrandType,
                            // include the industry name in the select clause


                            // Country = b.Country,
                            CountryName = c.Name, // select the country name from the Country table
                            //State = b.State,
                            StateName = s.Name,
                            // City = b.City,
                            CityName = ct.Name,

                            Zipcode = b.Zipcode,
                            //Address1 = b.Address1,
                            //Address2 = b.Address2,
                            //Province = b.Province,
                            //Logo = b.Logo,
                            //Material = b.Material,
                            Status = b.Status,
                            //AddedBy = b.AddedBy,
                            //AddedOn = b.AddedOn,
                            IndustryName = i.Name,


                        };
            return await query.ToListAsync();
        }
        public IEnumerable<ProspectDoc> GetMyData()
        {
            var query = from c in _context.Prospect
                        join o in _context.Countries on c.CountryId equals o.Id
                        select new ProspectDoc
                        {
                            ProspectName = c.Name,
                            IndustryType = c.IndustryType,
                            Country = c.CountryName,
                            State = c.State,
                            City = c.City,
                            Zipcode = c.Zip,
                            Address1 = c.Address1,
                            Address2 = c.Address2,
                            EmailAddress = c.Email,
                            ContactPerson = c.CPerson,
                            Phone = c.Number,
                            Website = c.Website,
                            Gst = c.Gst,
                            Source = c.Source,
                            Tax = o.TaxPerc,
                            LeadType = c.LeadType,
                        };

            return query.ToList();
        }

        public async Task DeleteCompany(int id)
        {
            try
            {
                var brand = await _context.Brand.FirstOrDefaultAsync(b => b.BrandId == id);
                _context.Brand.Remove(brand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting brand", ex);
            }

        }

        public async Task<Brand> GetById(int id)
        {
            return await _context.Brand.FirstOrDefaultAsync(b => b.BrandId == id);

        }

        public async Task<IEnumerable<Countries>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }



        public async Task<IEnumerable<States>> GetStates()
        {
            return await _context.States.ToListAsync();
        }
        public async Task<IEnumerable<resellerView>> GetAllResellers()
        {
            var resellers = (from distributor in _context.Distributor
                             join user in _context.User on distributor.UserId equals user.UserId
                             select new resellerView
                             {
                                 DistributorID = distributor.DisId,
                                 DID = distributor.DistributorId,
                                 FullName = user.UserFname,
                                 Email = user.UserMail,
                                 ContactNumber = distributor.DistributorNumber.ToString(),
                                 City = distributor.DistributorCity.ToString(),
                                 Country = distributor.DistributorCountry.ToString(),
                                 Status = user.UserStatus
                             });
            return await resellers.ToListAsync();
        }

        public async Task<IEnumerable<Cities>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<Product> AddProducts(Product product)
        {
            if (_context != null)
            {
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return product;

            }
            return null;
        }

        public async Task<Product> GetByproductId(int? id)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task EditProductDetails(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating products", ex);
            }
        }
        public async Task<IEnumerable<Industry>> GetAllIndustries()
        {
            return await _context.Industry.ToListAsync();
        }
        public async Task<Industry> GetIndustryById(int id)
        {
            return await _context.Industry.FindAsync(id);
        }
        public async Task<Industry> CreateIndustry(Industry industry)
        {
            if (_context != null)
            {
                _context.Industry.Add(industry);
                await _context.SaveChangesAsync();
                return industry;

            }
            return null;

        }
        //public async Task<Countries> CreateTaxes(Countries count)
        //{
        //    if (_context != null)
        //    {
        //        _context.Countries.Add(count);
        //        await _context.SaveChangesAsync();
        //        return count;

        //    }
        //    return null;
        //}
        public async Task<Countries> CreateTaxes(Countries countries)
        {

            var existingCountry = await _context.Countries.FindAsync(countries.Id);

            if (existingCountry != null)
            {
                existingCountry.TaxPerc = countries.TaxPerc;
                await _context.SaveChangesAsync();
                return existingCountry;
            }

            return null;
            //var country = await _context.Countries.FindAsync(countries.Id);
            ////string CountryName = country?.Name;
            //if (country != null)
            //{
            //    //country.TaxPerc = tax;
            //    _context.Entry(countries).State = EntityState.Modified;
            //    //_context.Countries.Add(country);
            //    await _context.SaveChangesAsync();
            //    return country;

            //}
            //return null;
        }

        public Task<Product> ProductDetails(int ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProduct(int ProductId)
        {
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(b => b.ProductId == ProductId);
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting brand", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetProductDetails()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetProductDetailsByID(int ProductId)
        {
            return await _context.Product.FindAsync(ProductId);

        }


        public async Task<Countries> Getcountrybyid(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task<IEnumerable<Industry>> GetIndustry()
        {
            return await _context.Industry.ToListAsync();
        }

        public async Task<Prospect> Createselfprospect(Prospect prospect)
        {
            if (_context != null)
            {
                _context.Prospect.Add(prospect);
                await _context.SaveChangesAsync();
                return prospect;

            }
            return null;

        }
        //public async Task<IEnumerable<resellerView>> GetAllResellers()
        //{
        //    var resellers = (from distributor in _context.Distributor
        //                     join user in _context.User on distributor.UserId equals user.UserId
        //                     select new resellerView
        //                     {
        //                         DistributorID = distributor.DisId,
        //                         DID = distributor.DistributorId,
        //                         FullName = user.UserFname,
        //                         Email = user.UserMail,
        //                         ContactNumber = distributor.DistributorNumber.ToString(),
        //                         City = distributor.DistributorCity.ToString(),
        //                         Country = distributor.DistributorCountry.ToString(),
        //                         Status = user.UserStatus
        //                     });
        //    return await resellers.ToListAsync();
        //}

        public async Task<Distributor> GetResellerById(int DistributorId)
        {
            return await _context.Distributor.FindAsync(DistributorId);

        }


        public async Task<Distributor> CreateReseller(Distributor reseller)
        {
            if (_context != null)
            {
                _context.Distributor.Add(reseller);
                await _context.SaveChangesAsync();
                return reseller;

            }
            return null;
        }
        public async Task UpdateReseller(Distributor reseller)
        {
            try
            {
                _context.Entry(reseller).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Reseller", ex);
            }
        }
        public async Task UpdateStatusReseller(User reseller)
        {
            try
            {
                _context.Entry(reseller).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating Reseller", ex);
            }
        }

        public async Task DeleteReseller(int Id)
        {
            try
            {
                var reseller = await _context.Distributor.FirstOrDefaultAsync(b => b.DisId == Id);
                _context.Distributor.Remove(reseller);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting brand", ex);
            }
        }
        public Task<IEnumerable<Prospect>> GetAllProspects()
        {
            throw new NotImplementedException();
        }

        public async Task<Prospect> insertprospects(Prospect prospect)
        {

            _context.Prospect.Add(prospect);
            await _context.SaveChangesAsync();
            return prospect;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task EditProspectDetails(Prospect prospect)
        {
            try
            {
                _context.Entry(prospect).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating prospects", ex);
            }
        }

        public async Task<Prospect> GetProspectDetailsByID(int ProspectId)
        {
            return await _context.Prospect.FindAsync(ProspectId);

        }

        
        public async Task<IEnumerable<InvoiceViewModel>> GetAllInvoices()
        {
            var invoices = (from Invoice in _context.PropInvoice
                            join prospect in _context.Prospect on Invoice.ProspectId equals prospect.Id
                            join pros_prod in _context.ProsProd on prospect.Id equals pros_prod.ProsId
                            join product in _context.Product on pros_prod.ProductId equals product.ProductId
                            join brand in _context.Brand on product.Brand equals brand.BrandId
                            select new InvoiceViewModel
                            {
                                Id = Invoice.Id,
                                InvoiceNo = Invoice.InvoiceId,
                                From = brand.BrandName,
                                To = prospect.Name,
                                Amount = Invoice.InvoiceAmt.ToString(),
                                Status = (int)prospect.Status,
                                AddedOn = prospect.AddedOn,
                                Licence = (int)Invoice.Licence
                            });
            return await invoices.ToListAsync();
        }
        public async Task<InvoiceViewModel> GetInvoicesById(int id)
        {
            return await _context.InvoiceViewModels.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<IEnumerable<Brand>> GetBrands()
        {
            return await _context.Brand.ToListAsync();
        }

        //public Task<Prospect> CreateResellerProspect(Prospect prospect)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task EditTaxDetails(Countries countries)
        //{
        //    try
        //    {
        //        _context.Entry(countries).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error updating taxes", ex);
        //    }
        //}

        //public async Task<Countries> GetTaxDetailsByID(int Id)
        //{
        //    return await _context.Countries.FindAsync(Id);
        //}

       

        public async Task<Zip> CreateTaxes(Zip zip)
        {
            if (_context != null)
            {
                _context.Zip.Add(zip);
                await _context.SaveChangesAsync();
                return zip;

            }
            return null;
        }

       

        
        public async Task<ResellerDetailsView> GetResellersDetailsView(int DistributorID)
        {
            var ResellerDetails = (from distributor in _context.Distributor
                                   join user in _context.User on distributor.UserId equals user.UserId
                                   join countries in _context.Countries on distributor.DistributorCountry equals countries.Id
                                   join zip in _context.Zip on distributor.DistributorZip equals zip.Zipcode
                                   join states in _context.States on distributor.DistributorState equals states.Id
                                   join cities in _context.Cities on distributor.DistributorCity equals cities.Id
                                   join distri_doc in _context.DistriDocu on distributor.DisId equals distri_doc.Id
                                   select new ResellerDetailsView
                                   {
                                       DisID = distributor.DisId,
                                       DistributorId = distributor.DistributorId,
                                       DistributorName = distributor.DistributorName,
                                       DistributorType = distributor.DistributorType,
                                       Country = countries.Name,
                                       State = states.Name,
                                       City = cities.Name,
                                       Zipcode = zip.Zipcode,
                                       Address1 = distributor.DistributorAddress1,
                                       Address2 = distributor.DistributorAddress2,
                                       Email = user.UserMail,
                                       ContactPersonName = distributor.DistributorCName,
                                       PhoneNo = distributor.DistributorNumber.ToString(),
                                       VAT = distributor.DistributorVat,
                                       Website = distributor.Website,
                                   });
            return await ResellerDetails.FirstOrDefaultAsync();
        }

        public async Task<DistriDocu> GetDistriDocuByID(int Id)
        {
            return await _context.DistriDocu.FindAsync(Id);

        }

        public async Task UpdateDistriDocu(DistriDocu distriDocu)
        {
            try
            {
                _context.Entry(distriDocu).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating documents", ex);
            }
        }
        public async Task<IEnumerable<Zip>> GetAllZipCodes()
        {
            return await _context.Zip.ToListAsync();
        }



        public async Task<Zip> CreateZipCode(Zip zip)
        {
            if (_context != null)
            {
                _context.Zip.Add(zip);
                await _context.SaveChangesAsync();
                return zip;

            }
            return null;
            //if (_context != null)
            //{
            //    _context.Industry.Add(industry);
            //    await _context.SaveChangesAsync();
            //    return industry;

            //}
            //return null;
        }


        public async Task EditZipDetails(Zip zip)
        {
            try
            {
                _context.Entry(zip).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating taxes", ex);
            }
        }


        public async Task<Zip> GetZipDetailsByID(int Id)
        {
            return await _context.Zip.FindAsync(Id);

        }

        public async Task<ProductMapView> GetProductMapById(int RId)
        {
            var productMap = (from product in _context.ProductMap
                              join distributor in _context.Distributor on product.RId equals distributor.DisId
                              join products in _context.Product on product.ProductId equals products.ProductId
                              select new ProductMapView
                              {
                                  ProductName=products.ProductName,
                                  FromQty=(int)products.FromNode,
                                  ToQty=(int)products.ToNode,
                                  UnitPrice=products.ProductPrice,
                                  Discount=(int)product.Discount,
                                  AfterDiscount=(int)product.ADiscount
                              });
            return await productMap.FirstOrDefaultAsync();
        }
        public async Task UpdateIndustryDetails(Industry indu)
        {
            if (_context != null)
            {
                var play = _context.Industry.Update(indu);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Industry> DetailsIndustry(int? id)
        {
            if (_context != null)
            {
                return await _context.Industry.FindAsync(id);
            }
            return null;
        }
        public async Task<Countries> EditTaxDetails(Countries countries)
        {


            var existingCountry = await _context.Countries.FindAsync(countries.Id);

            if (existingCountry != null)
            {
                existingCountry.TaxPerc = countries.TaxPerc;
                await _context.SaveChangesAsync();
                return existingCountry;
            }
            return null;


            //try
            //{
            //    _context.Entry(countries).State = EntityState.Modified;
            //    await _context.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error updating taxes", ex);
            //}
        }

        public async Task<Countries> GetTaxDetailsByID(int Id)
        {
            return await _context.Countries.FindAsync(Id);
        }
        public async Task<Prospect> CreateResellerProspect(Prospect prospect)
        {
            if (_context != null)
            {
                _context.Prospect.Add(prospect);
                await _context.SaveChangesAsync();
                return prospect;

            }
            return null;
        }
        public IEnumerable<Product> GetMyDatas()
        {
            var query = from c in _context.Product
                            //join o in _context.Countries on c.CountryId equals o.Id
                        select new Product
                        {

                            ProductCode = c.ProductCode,
                            Brand = c.Brand,
                            Model = c.Model,
                            Variant = c.Variant,
                            ProductName = c.ProductName,
                            ProductDesc = c.ProductDesc,
                            ProductImg = c.ProductImg,
                            ProductPrice = c.ProductPrice,
                            FromNode = c.FromNode,
                            ToNode = c.ToNode,
                            AddedOn = c.AddedOn,
                            AddedBy = c.AddedBy,
                            Status = c.Status


                        };

            return query.ToList();
        }

    }
}