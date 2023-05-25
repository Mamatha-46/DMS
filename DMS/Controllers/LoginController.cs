using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;
using DMS.Repository;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
//using DMS.Models;
//using DMS.Models;

namespace DMS.Controllers
{
    public class LoginController : Controller
    {
        private ILogin _ILogin;
        private IPostRepository _ipos;
        private readonly cyberContext _context;
        public LoginController(cyberContext cyberContext, IPostRepository ipos)
        {
            _ipos = ipos;
            _context = cyberContext;
            _ILogin = new LoginRepository();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var UserName = model.UserName;

            var Password = model.Password;

            var user = _ILogin.ValidateUser(UserName,Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UName", UserName.ToString());
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
        }
        public async Task<IActionResult> Create()
        {
            var distributorTypes = await _ipos.GetIndustry();
            ViewBag.DistributorTypes = new SelectList(distributorTypes, "Id", "Name");
            ViewData["Type"] = ViewBag.DistributorTypes;

            var countr = await _ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            var state = await _ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await _ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            //var countries = await ipos.GetCountries();
            //ViewBag.Countries = new SelectList(countries, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistributorViewmodel viewModel)
        {
            // Check if the ViewModel data is valid
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

                        UserLogo = viewModel.UserLogo,
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


                    var distriDocu = new DistriDocu
                    {
                        DistriId = lastmodel.DisId + 1,
                        Business = viewModel.CompanyDocument,
                        Opened = 0, // Assuming a default value for opened field
                        Status = 1, // Assuming a default status value
                        Gst=viewModel.Taxdocumebt,
                        AddedOn = DateTime.Now,
                        AddedBy = "self" // Assuming a default value for the added by field
                    };
                    _context.User.Add(user);
                    _context.Distributor.Add(distributor);
                    _context.DistriDocu.Add(distriDocu);
                    _context.SaveChanges();

                    // Redirect to a success page or perform any other action
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            // If the ViewModel data is not valid, return to the create view
            return View(viewModel);


        }

        //to get state 
        public IActionResult GetStatesByCountryId(int countryId)
        {
            var states = _context.States
                .Where(s => s.CountryId == countryId)
                .ToList();
            var stateList = states.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            return Json(stateList);
        }

        //to get city
        public IActionResult GetcitiesBystateId(int stateId)
        {
            var states = _context.Cities
                .Where(s => s.StateId == stateId)
                .ToList();
            var stateList = states.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            return Json(stateList);
        }


        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Distributor dist,User use)
        //{

        //    _ILogin.addDistributor(dist,use);
        //    return RedirectToAction("Login");

        //}

    }
}
