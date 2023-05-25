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
using OfficeOpenXml;

namespace DMS.Controllers
{
    public class ProspectsController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly cyberContext context;


        public ProspectsController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 7)
        {
            var brandinfo = await ipos.GetProspectDetails();
            //return View(brandinfo);
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
                    b != null &&
                    (b.UserFname != null && b.UserFname.Contains(searchString)) ||
                    (b.Name != null && b.Name.Contains(searchString)) ||
                    (b.CPerson != null && b.CPerson.Contains(searchString)) ||
                      (b.Email != null && b.Email.Contains(searchString)) ||
                    (b.CountryName != null && b.CountryName.Contains(searchString))


                //(b.Number != null && b.Number.ToString() != null && b.Number.ToString().Contains(searchString)) ||



                );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.UserFname).ToList();
                    break;
                case "type":
                    brandinfo = brandinfo.OrderBy(b => b.Name).ToList();
                    break;
                case "type_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Name).ToList();
                    break;
                case "CPerson":
                    brandinfo = brandinfo.OrderBy(b => b.CPerson).ToList();
                    break;
                case "CPerson_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.CPerson).ToList();
                    break;
                case "Email":
                    brandinfo = brandinfo.OrderBy(b => b.Email).ToList();
                    break;
                case "Email_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Email).ToList();
                    break;
                default:
                    brandinfo = brandinfo.OrderBy(b => b.UserFname);
                    break;
            }

            //var viewModel = new brand
            //{
            //    Brands = brandinfo.ToList(),
            //    SearchString = searchString,
            //    SortOrder = sortOrder
            //};

            //return View(brandinfo);
            var pagedData = await brandinfo.ToPagedListAsync(page, pageSize);

            // Pass the paged data to the view
            return View(pagedData);







            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);



        }

            public async Task<IActionResult> CreateselfProspect()
        {
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["CountryName"] = ViewBag.Countries;
            var prospect = new Prospect();
            prospect.CountryId = countr.FirstOrDefault()?.Id; // set default country id
            prospect.CountryName = countr.FirstOrDefault()?.Name; // set default country name

            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await ipos.GetCities();

            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["IndustryType"] = ViewBag.Industry;
            return View(prospect);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateselfProspect(Prospect prospect)
        {
            await ipos.Createselfprospect(prospect);
            return PartialView("Createselfprospect");
        }
        
        
        public async Task<IActionResult> CreateResellerProspect()
        {
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["CountryName"] = ViewBag.Countries;
            var prospect = new Prospect();
            prospect.CountryId = countr.FirstOrDefault()?.Id; // set default country id
            prospect.CountryName = countr.FirstOrDefault()?.Name; // set default country name

            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await ipos.GetCities();

            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["IndustryType"] = ViewBag.Industry;
            var Users = await ipos.GetAllusers();
            ViewBag.User = new SelectList(Users, "UserId", "UserFname");
            ViewData["AddedId"] = ViewBag.User;
            //var products = await ipos.GetProducts();
            //ViewBag.Product = new SelectList(products, "UserId", "UserFname");
            //ViewData["AddedId"] = ViewBag.User;
            var products = await ipos.GetProducts();
            ViewBag.Product = new SelectList(products, "ProductId", "ProductName");
            ViewData["product-id"] = ViewBag.Product;
            //var model = new prospectviewmodel();
            return View(prospect);
        }
        [HttpPost]
        public async Task<string> UploadFile(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }



        [HttpPost]
        public async Task<IActionResult> CreateResellerProspect(Prospect prospect, IFormFile logo)
        {

            //await ipos.insertprospects(prospect);
            // return RedirectToAction("Index");
            try
            {
                if (ModelState.IsValid)
                {
                    if (logo != null && logo.Length > 0)
                    {
                        // Get the file extension
                        var extension = Path.GetExtension(logo.FileName);

                        // Generate a unique filename using a GUID
                        var filename = $"{Guid.NewGuid()}{extension}";

                        // Set the path to save the file
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);

                        // Save the file to disk
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await logo.CopyToAsync(stream);
                        }

                        // Set the brand logo path to the uploaded file
                        prospect.Logo = $"/uploads/{filename}";

                    }


                    await ipos.insertprospects(prospect);
                    return RedirectToAction("Index");
                }


                return View("createproduct");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var prospect = await ipos.GetProspectDetailsByID(id.Value);
            if (prospect == null)
            {
                return NotFound();
            }
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["CountryName"] = ViewBag.Countries;
            //var prospect = new Prospect();
            prospect.CountryId = countr.FirstOrDefault(c => c.Name == prospect.CountryName)?.Id;
            // set default country id
            prospect.CountryName = countr.FirstOrDefault()?.Name; // set default country name


            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await ipos.GetCities();

            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["IndustryType"] = ViewBag.Industry;
            var Users = await ipos.GetAllusers();
            ViewBag.User = new SelectList(Users, "UserId", "UserFname");
            ViewData["AddedId"] = ViewBag.User;
            var products = await ipos.GetProducts();
            ViewBag.Product = new SelectList(products, "ProductId", "ProductName");
            ViewData["product-id"] = ViewBag.Product;


            return View(prospect);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prospect prospect)
        {
            if (id != prospect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                prospect.Logo = "null";
                prospect.UpdatedOn = DateTime.Now;
                try
                {

                    await ipos.EditProspectDetails(prospect);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View(prospect);
        }

        public IActionResult RadioButton()
        {

            return View();

        }
        public async Task<IActionResult> Self()
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
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["BrandType"] = ViewBag.Industry;
            //var users = await ipos.GetAllusers();
            //ViewBag.Industry = new SelectList(industries, "Id", "Name");
            //ViewData["BrandType"] = ViewBag.Industry;




            var model = new ProspectSelfViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Self(ProspectSelfViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pros = new Prospect
                {

                    Name = model.Name,
                    IndustryType = model.IndustryType,
                    CountryName = model.CountryName,
                    State = model.State,
                    City = model.City,
                    Zip = model.Zip,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Email = model.Email,
                    CPerson = model.CPerson,
                    Number = model.Number,
                    Website = model.Website,
                    Gst = model.Gst,
                    Source = model.Source,
                    LeadType = model.LeadType,
                    Status = model.Status,
                    Terms = model.Terms,
                    Logo = "null"

                };
                await ipos.Createselfprospect(pros);
                return RedirectToAction("Index");
            }


            return View(model);
        }


        public async Task<IActionResult> ResellerCreate()
        {
            var _dbContext = new cyberContext();

            //var use = await ipos.GetAllusers();
            ////var users = _dbContext.User.Where(u => u.UserFname == "Employee").ToList();
            ////ViewBag.EmployeeUsers = users;
            //ViewBag.User = new SelectList(use, "UserId", "UserFname");
            //ViewData["User"] = ViewBag.users;
            var use = await ipos.GetAllusers();
            ViewBag.User = new SelectList(use, "UserId", "UserFname");

            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["BrandType"] = ViewBag.Industry;
            //var users = await ipos.GetAllusers();
            //ViewBag.Industry = new SelectList(industries, "Id", "Name");
            //ViewData["BrandType"] = ViewBag.Industry;

            var model = new ProspectResellerViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResellerCreate(ProspectResellerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pros = new Prospect
                {
                    AddedId = model.AddedId,
                    Name = model.Name,
                    IndustryType = model.IndustryType,
                    CountryName = model.CountryName,
                    State = model.State,
                    City = model.City,
                    Zip = model.Zip,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Email = model.Email,
                    CPerson = model.CPerson,
                    Number = model.Number,
                    Website = model.Website,
                    Gst = model.Gst,
                    Source = model.Source,
                    LeadType = model.LeadType,
                    Status = model.Status,
                    Terms = model.Terms,
                    Logo = "null"
                };
                await ipos.CreateResellerProspect(pros);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        //public IActionResult GetStatesByCountryId(int countryId)
        //{
        //    var states = context.States
        //        .Where(s => s.CountryId == countryId)
        //        .ToList();
        //    var stateList = states.Select(s => new SelectListItem
        //    {
        //        Value = s.Id.ToString(),
        //        Text = s.Name
        //    }).ToList();

        //    return Json(stateList);
        //}

        //public IActionResult GetcitiesBystateId(int stateId)
        //{
        //    var states = context.Cities
        //        .Where(s => s.StateId == stateId)
        //        .ToList();
        //    var stateList = states.Select(s => new SelectListItem
        //    {
        //        Value = s.Id.ToString(),
        //        Text = s.Name
        //    }).ToList();

        //    return Json(stateList);
        //}
    }


    //[HttpGet]
    //public IActionResult ExportToExcel()
    //{
    //    var data = ipos.GetMyData();
    //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    //    using (var package = new ExcelPackage())
    //    {
    //        var worksheet = package.Workbook.Worksheets.Add("My Data");

    //        // Write the column headers
    //        worksheet.Cells[1, 1].Value = "Column 1";
    //        worksheet.Cells[1, 2].Value = "Column 2";
    //        worksheet.Cells[1, 3].Value = "Column 3";

    //        // Write the data
    //        var row = 2;
    //        foreach (var item in data)
    //        {
    //            worksheet.Cells[row, 1].Value = item.Name;
    //            worksheet.Cells[row, 2].Value = item.Number;
    //            worksheet.Cells[row, 3].Value = item.Email;
    //            row++;
    //        }

    //        // Create a memory stream and save the package to it
    //        var stream = new MemoryStream();
    //        package.SaveAs(stream);

    //        // Set the position of the stream back to the beginning
    //        stream.Seek(0, SeekOrigin.Begin);

    //        // Return the Excel file as a file download
    //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "My Data.xls");
    //    }


}    

