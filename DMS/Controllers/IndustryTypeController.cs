using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;
using DMS.Repository;
using Microsoft.EntityFrameworkCore;
//using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using X.PagedList;


namespace DMS.Controllers
{
    public class IndustryTypeController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IndustryTypeController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 9)
        {
            // int pageSize = 10;
            // List<Brand> brandinfo = new List<Brand>();

            var brandinfo = await ipos.GetAllIndustries();
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
    (b.Name != null && b.Name.Contains(searchString)));


            }
            switch (sortOrder)
            {
                case "name_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Name);
                    break;
            }
            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);
            var pagedData = await brandinfo.ToPagedListAsync(page, pageSize);


            return View(pagedData);

        }
        public async Task<IActionResult> zip(string searchString, string sortOrder)
        {
            // int pageSize = 10;
            // List<Brand> brandinfo = new List<>();
            var brandinfo = await ipos.GetAllZipCodes();
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
                    b != null &&
                    (b.Country != null && b.Country.Contains(searchString)) ||
                    (b.State != null && b.State.Contains(searchString)) ||
                    (b.City != null && b.City.Contains(searchString)) ||
                    (b.Zipcode != null && b.Zipcode.ToString() != null && b.Zipcode.ToString().Contains(searchString)));

            }

            switch (sortOrder)
            {
                case "country_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Country);
                    break;
                case "state_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.State);
                    break;
                case "city_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.City);
                    break;

            }
            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);

            return View(brandinfo);

        }

        public async Task<IActionResult> tax(string searchString, string sortOrder, int page = 1, int pageSize = 15)
        {
            // int pageSize = 10;
            // List<Brand> brandinfo = new List<>();
            var brandinfo = await ipos.GetCountries();
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    brandinfo = brandinfo.Where(b =>
            //        b != null &&
            //        (b.Name != null && b.Name.Contains(searchString)) ||
            //        //(b.TaxPerc != null && b.TaxPerc.Contains(searchString)) ||
            //        (b.TaxPerc != null && b.TaxPerc.ToString() != null && b.TaxPerc.ToString().Contains(searchString)));

            //}
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
                    b != null &&
                    (b.Name != null && b.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0));
               
            }
            switch (sortOrder)
            {
                case "name_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Name);
                    break;
            }
            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);
            var pagedData = await brandinfo.ToPagedListAsync(page, pageSize);

            return View(pagedData);
        }
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var brand = await ipos.GetIndustryById(id);
            if (brand == null)
            {
                return NotFound();
            }
            brand.Status = (brand.Status == 1) ? 0 : 1;
            //await ipos.UpdateCompany(brand);
            TempData["Message"] = (brand.Status == 0) ? "Record activated successfully." : "Record deactivated successfully.";
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> ZipCreate()
        //{
        //    var countr = await ipos.GetCountries();
        //    ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["CountryId"] = ViewBag.Countries;
        //    var state = await ipos.GetStates();
        //    ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["StateId"] = ViewBag.States;
        //    var city = await ipos.GetCities();

        //    ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["CityId"] = ViewBag.Cities;
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ZipCreate(Countries count)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            await ipos.CreateTaxes(count);
        //            //return RedirectToAction("Index");
        //        }
        //        return View("taxCreate");
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
        public async Task<IActionResult> taxCreate()
        {
            var countries = await ipos.GetCountries();
            var countrySelectList = new SelectList(countries, "Id", "Name");
            ViewData["Id"] = countrySelectList;

            return View();
        }

        //public async Task<IActionResult> taxCreate()
        //{
        //    var countr = await ipos.GetCountries();
        //    ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["Name"] = ViewBag.Countries;
        //    return View();
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> taxCreate(Countries countries)
        {

            try
            {

                    var item =  await ipos.CreateTaxes(countries);
                    if (item == null)
                    {
                        return NotFound();
                    }
                var countriesList = await ipos.GetCountries();
                var countrySelectList = new SelectList(countriesList, "Id", "Name");
                ViewData["Id"] = countrySelectList;

                return RedirectToAction("tax");
                //return RedirectToAction("Index");

                //return View("tax");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<IActionResult> TaxEdit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await ipos.GetTaxDetailsByID(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(brand);
        //}

        // POST: Brand/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> TaxEdit(int id, Countries countries)
        //{
        //    if (id != countries.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ipos.EditTaxDetails(countries);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(countries);
        //}

        //public async Task<IActionResult> TaxDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await ipos.GetTaxDetailsByID(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand);
        //}
        //public async Task<IActionResult> ZipEdit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await ipos.GetZipDetailsByID(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(brand);
        //}

        //// POST: Brand/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ZipEdit(int id, Zip zip)
        //{
        //    if (id != zip.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ipos.EditZipDetails(zip);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(zip);
        //}

        //public async Task<IActionResult> zipDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await ipos.GetZipDetailsByID(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(brand);
        //}

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Industry industry)
        {
            try
            {
                var play = await ipos.CreateIndustry(industry);
                if (play == null)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
                //if (ModelState.IsValid)
                //{
                //    await ipos.CreateIndustry(industry);
                //    //return RedirectToAction("Index");
                //}


                //return View("CreateIndustry");

            }
            catch
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> TaxEdit(int? id)
        {

            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            if (id == null)
            {
                return NotFound();
            }

            var brand = await ipos.GetTaxDetailsByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaxEdit(int id, Countries countries)
        {
            if (id != countries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ipos.EditTaxDetails(countries);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("tax");
            }
            return View(countries);
        }
        public async Task<IActionResult> TaxDetails(int? id)
        {
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            if (id == null)
            {
                return NotFound();
            }

            var brand = await ipos.GetTaxDetailsByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var brand = await ipos.GetIndustryById(Id);
            if (brand == null)
            {
                return NotFound();
            }
            var model = new IndustryViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                Status = brand.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IndustryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var brand = await ipos.GetIndustryById(id);
                if (brand == null)
                {
                    return NotFound();
                }

                brand.Name = model.Name;
                brand.Status = model.Status;

                await ipos.UpdateIndustryDetails(brand);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Details(int? id, IndustryViewModel model)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var brand = await ipos.GetIndustryById(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);


        }
    }
}
