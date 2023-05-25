using DMS.Models;
using DMS.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; 

namespace DMS.Controllers
{
    public class ZipController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ZipController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
        }

       

        public async Task<IActionResult> Index2()
        {
              var _context = new cyberContext();
              var countr = await ipos.GetCountries();
              ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
              ViewData["Country"] = ViewBag.Countries;
              var state = await ipos.GetStates();
              ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
              ViewData["State"] = ViewBag.States;
              var city = await ipos.GetCities();
              ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
              ViewData["City"] = ViewBag.Cities;

            return View();

        }

        [HttpGet]
        public ActionResult Search(int country, int state, int city)
        {
            var _context = new cyberContext();

            List<Zip> searchResults = new List<Zip>();
           

            if (country != 0 && state == 0 && city == 0)
            {
                searchResults = _context.Zip.Where(z => z.CountryId == country).ToList();
            }
            else if (country != 0 && state != 0 && city == 0)
            {
                searchResults = _context.Zip.Where(z => z.CountryId == country && z.StateId == state).ToList();
            }
            else if (country != 0 && state != 0 && city != 0)
            {
                searchResults = _context.Zip.Where(z => z.CountryId == country && z.StateId == state && z.CityId == city).ToList();
            }

            return PartialView("_Search", searchResults);
        }


        public IActionResult GetSearchIndex(int country, int state, int city, int zip)
        {
            var _context = new cyberContext();

            var query = _context.Zip.Where(z => z.CountryId == country);

            //var q = from z in _context.Zip
            //        where z.Country == country
            //        select new ZipModel
            //        (
            //            Contry = z.

            //            );
            if (state != 0)
            {
                query = query.Where(z => z.StateId == state);
            }

            if (city != 0)
            {
                query = query.Where(z => z.CityId == city);
            }


            var result = query.ToList();
            return View();
        }
        public IActionResult GetStatesByCountryId(int countryId)
        {
            var _context = new cyberContext();
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
        public IActionResult GetcitiesBystateId(int stateId)
        {
            var _context = new cyberContext();
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
        public async Task<IActionResult> Create()
        {
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;

            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;

            var city = await ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;

            var model = new Zip();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZipRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                var brand = new Zip
                {
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    Zipcode = model.Zipcode,
                    Division1 = model.Division1,
                    Division2 = model.Division2,
                    AreaName = model.AreaName,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,

                };
                await ipos.CreateZipCode(brand);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> ZipEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await ipos.GetZipDetailsByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZipEdit(int id, Zip zip)
        {
            if (id != zip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ipos.EditZipDetails(zip);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View(zip);
        }
        public async Task<IActionResult> ZipDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await ipos.GetZipDetailsByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

    }



}
