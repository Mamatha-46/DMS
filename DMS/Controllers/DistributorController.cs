using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Repository;
using DMS.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMS.Controllers
{
    public class DistributorController : Controller
    {
        private readonly IPostRepository _ipr;
        cyberContext _context;
        public DistributorController(IPostRepository ipr, cyberContext context)
        {
            _context = context;
            _ipr = ipr;

        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pagesize = 5)
        {
            var brandinfo = await _ipr.GetAllResellers();
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
                (b.DID != null && b.DID.Contains(searchString)) ||
                 (b.FullName != null && b.FullName.Contains(searchString)) ||
                 (b.Country != null && b.Country.ToString().Contains(searchString)) ||
                 (b.Status.ToString() != null && b.Status.ToString().Contains(searchString)) ||
                 (b.City != null && b.City.ToString().Contains(searchString)) ||
                 (b.ContactNumber != null && b.ContactNumber.ToString().Contains(searchString)));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.DID);
                    break;
                case "type":
                    brandinfo = brandinfo.OrderBy(b => b.FullName);
                    break;
                case "type_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.FullName);
                    break;
                case "country":
                    brandinfo = brandinfo.OrderBy(b => b.Email);
                    break;
                case "country_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Email);
                    break;
                case "state":
                    brandinfo = brandinfo.OrderBy(b => b.ContactNumber);
                    break;
                case "state_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.ContactNumber);
                    break;
                case "city":
                    brandinfo = brandinfo.OrderBy(b => b.City);
                    break;
                case "city_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.City);
                    break;
                case "zipcode":
                    brandinfo = brandinfo.OrderBy(b => b.Country);
                    break;
                case "zipcode_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Country);
                    break;
                default:
                    brandinfo = brandinfo.OrderBy(b => b.DID);
                    break;
            }
            var pagedata = await brandinfo.ToPagedListAsync(page, pagesize);
            return View(pagedata);
        }


        //public async Task<IActionResult> Create()
        //{
        //    var coutr  
        //}

        public async Task<IActionResult> CreateReseller()
        {
            var distributorTypes = await _ipr.GetIndustry();
            ViewBag.DistributorTypes = new SelectList(distributorTypes, "Id", "Name");
            ViewData["Type"] = ViewBag.DistributorTypes;

            var countr = await _ipr.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            var state = await _ipr.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await _ipr.GetCities();

            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateReseller(DistributorViewmodel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Create instances of the models and map the ViewModel data
                    var user = new User
                    {
                        UserFname = viewModel.DistributorName,
                        UserMail = viewModel.Email,
                        Pwd = "Pass@123",
                        UserNumber = viewModel.DistributorNumber,
                        //UserType = viewModel.DistributorType,
                        UserType = "R",

                        UserLogo = "null",
                        UserStatus = 2, // Assuming a default status value
                        AddedOn = DateTime.Now,
                        UserPic = "null",
                        UserPwd = "null",
                        Bank = "null",
                        Readed = 0,
                        AReaded = null
                    };
                    //_context.User.Add(user);
                    // Get the last user model and generate a new UserId.
                    var item = _context.User.ToList();
                
                    var lastitem = item.Last();
                    // Get the last distributor model and generate a new distributor ID.
                    var model = _context.Distributor.ToList();
                    var lastmodel = model.Last();
                    int incrementedNumber = 1;
                    if (lastmodel != null)
                    {
                        string numericPortion = lastmodel.DistributorId.Replace("RID-", "");
                        int lastNumber = int.Parse(numericPortion);
                        incrementedNumber = lastNumber + 1;
                    }

                    string newDistributorId = "RID-" + incrementedNumber.ToString("D3");





                    var distributor = new Distributor
                    {
                        UserId = lastitem.UserId + 1,
                        DistributorId = newDistributorId,
                        DistributorName = viewModel.DistributorName,
                        Website = viewModel.Website,
                        DistributorType = viewModel.DistributorType,
                        DistributorZip = viewModel.DistributorZip,
                        DistributorCountry = viewModel.DistributorCountry,
                        DistributorState = viewModel.DistributorState,
                        DistributorCity = viewModel.DistributorCity,
                        DistributorAddress1 = viewModel.DistributorAddress1,
                        DistributorAddress2 = viewModel.DistributorAddress2,
                        DistributorCName = viewModel.DistributorCName,
                        DistributorNumber = viewModel.DistributorNumber,
                        DistributorVat = viewModel.DistributorVat,
                        AddedOn = DateTime.Now,
                        AddedBy = "self" // Assuming a default value for the added by field
                    };
                    //_context.Distributor.Add(distributor);


                    //var distriDocu = new DistriDocu
                    //{
                    //    DistriId = lastmodel.DisId + 1,
                    //    Business = viewModel.CompanyDocument,
                    //    Opened = 0, // Assuming a default value for opened field
                    //    Status = 1, // Assuming a default status value
                    //    AddedOn = DateTime.Now,
                    //    AddedBy = "self" // Assuming a default value for the added by field
                    //};
                    _context.User.Add(user);
                    _context.Distributor.Add(distributor);
                    //_context.DistriDocu.Add(distriDocu);
                    _context.SaveChanges();

                    // Redirect to a success page or perform any other action
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // If the ViewModel data is not valid, return to the create view
            return View();

        }
        public async Task<IActionResult> UpdateReseller(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _ipr.GetResellerById(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateReseller(int DistributorId, Distributor reseller)
        {
            if (DistributorId != reseller.DisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ipr.UpdateReseller(reseller);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View(reseller);
        }
        public async Task<IActionResult> DeleteReseller(int id)
        {
            try
            {
                var reseller = await _ipr.GetResellerById(id);
                if (reseller == null)
                {
                    return NotFound();
                }

                await _ipr.DeleteReseller(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
                // Handle exception and return error view
            }
        }
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var reseller = await _ipr.GetUserById(id);
            if (reseller == null)
            {
                return NotFound();
            }

            reseller.UserStatus = (reseller.UserStatus == 1) ? 0 : 1;
            await _ipr.UpdateStatusReseller(reseller);

            TempData["Message"] = (reseller.UserStatus == 0) ? "Record activated successfully." : "Record deactivated successfully.";

            return RedirectToAction("Index");
        }
        
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reseller = await _ipr.GetResellerById(id.Value);
        //    if (reseller == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reseller);
        //}

        public async Task<IActionResult> Details(int? DistributorID)
        {
            if (DistributorID == null)
            {
                return NotFound();
            }
            var reseller = await _ipr.GetResellersDetailsView(DistributorID.Value);
            if (reseller == null)
            {
                return NotFound();
            }
            return View(reseller);
        }

        public async Task<IActionResult> UpdateDistriDocu(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _ipr.GetDistriDocuByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return PartialView("_PartialDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDistriDocu(int DistriId, DistriDocu distriDocu)
        {
            if (DistriId != distriDocu.DistriId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ipr.UpdateDistriDocu(distriDocu);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return PartialView("_PartialDetails");
        }

        public async Task<IActionResult> GetProductMapById(int? RID)
        {
            if (RID == null)
            {
                return NotFound();
            }
            var reseller = await _ipr.GetProductMapById(RID.Value);
            if (reseller == null)
            {
                return NotFound();
            }
            return View(reseller);
        }
    }
}