using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Repository
{
    public interface IPostRepository
    {
        Task<IEnumerable<User>> GetAllusers();
        Task<Product> GetByproductId(int? id);
        IEnumerable<Product> GetMyDatas();
        Task AddUser(User user);
        Task<User> GetUserById(int UserId);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task<Brand> Createcompany(Brand brand);
        Task<Prospect> Createselfprospect(Prospect prospect);
        Task<Prospect> CreateResellerProspect(Prospect prospect);
        Task<Prospect> insertprospects(Prospect prospect);
        Task<IEnumerable<Prospect>> GetAllProspects();
        Task<IEnumerable<brandviewmodel>> GetAllCompanies();
        
        Task UpdateCompany(Brand brand);
        Task DeleteCompany(int id);
        Task<Brand> GetById(int id);
        Task<Countries> Getcountrybyid(int id);
        Task<IEnumerable<Countries>> GetCountries();
        Task<IEnumerable<Brand>> GetBrands();
       
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<States>> GetStates();
        Task<IEnumerable<Cities>> GetCities();
        Task<IEnumerable<Industry>> GetIndustry();
        Task<Product> AddProducts(Product product);

        Task<IEnumerable<Product>> GetProductDetails();
        Task<IEnumerable<prospectviewmodel>> GetProspectDetails();

        Task EditProductDetails(Product product);
        //Task EditTaxDetails(Countries countries);
        

        Task EditProspectDetails(Prospect prospect);
        Task<Prospect> GetProspectDetailsByID(int ProspectId);

        Task<Product> GetProductDetailsByID(int ProductId);

        //void Save();

        Task<Product> ProductDetails(int ProductId);
        Task DeleteProduct(int ProductId);

        IEnumerable<ProspectDoc> GetMyData();
        Task<IEnumerable<Industry>> GetAllIndustries();
        
        Task<Industry> GetIndustryById(int Id);
       // Task<Countries> GetTaxDetailsByID(int Id);
        Task<Industry> CreateIndustry(Industry industry);
        Task<Countries> CreateTaxes(Countries countrires);
        Task<Zip> CreateTaxes(Zip zip);
        //Task<IEnumerable<Zip>> GetAllZip();
        //for reseller
        Task<IEnumerable<resellerView>> GetAllResellers();
        Task<Distributor> GetResellerById(int DistributorId);
        Task<Distributor> CreateReseller(Distributor reseller);
        Task UpdateReseller(Distributor reseller);
        Task UpdateStatusReseller(User reseller);
        Task DeleteReseller(int id);
        Task<IEnumerable<InvoiceViewModel>> GetAllInvoices();
        Task<InvoiceViewModel> GetInvoicesById(int Id);
        Task<ResellerDetailsView> GetResellersDetailsView(int Id);
        Task UpdateDistriDocu(DistriDocu distriDocu);
        Task<DistriDocu> GetDistriDocuByID(int Id);
        Task<Zip> GetZipDetailsByID(int Id);
        Task<Zip> CreateZipCode(Zip zip);
        Task EditZipDetails(Zip zip);
        Task<IEnumerable<Zip>> GetAllZipCodes();
        Task<ProductMapView> GetProductMapById(int ProductMapId);
        Task UpdateIndustryDetails(Industry indu);
        Task<Industry> DetailsIndustry(int? id);
        Task<Countries> EditTaxDetails(Countries countries);
        Task<Countries> GetTaxDetailsByID(int Id);

        Task<DistDocView> updatedocstatus(DistDocView dic);

    }

}
