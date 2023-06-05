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


        public async Task<IActionResult> RadioButton()
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

            //var industries = await ipos.GetIndustry();
            //ViewBag.Industry = new SelectList(industries, "Id", "Name");

            var model = new ProspectSelfViewModel();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RadioButton(ProspectSelfViewModel model)
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

                //var pro = new ProsProd
                //{
                //    ProsId=prop.ProsId,
                //    ProductId=prop.ProductId,
                //    Discount=prop.Discount,
                //    EQty=prop.EQty,
                //    UPrice=prop.UPrice,
                //    STotal=prop.STotal,
                //    AddedOn=DateTime.Now
                //    //Name = model.Name,
                //    //IndustryType = model.IndustryType,
                //    //CountryName = model.CountryName,
                //    //State = model.State,
                //    //City = model.City,
                //    //Zip = model.Zip,
                //    //Address1 = model.Address1,
                //    //Address2 = model.Address2,
                //    //Email = model.Email,
                //    //CPerson = model.CPerson,
                //    //Number = model.Number,
                //    //Website = model.Website,
                //    //Gst = model.Gst,
                //    //Source = model.Source,
                //    //LeadType = model.LeadType,
                //    //Status = model.Status,
                //    //Terms = model.Terms,
                //    //Logo = "null"

                //};
                await ipos.Createselfprospect(pros);
                //await ipos.CreateProductprospect(pro);
                return RedirectToAction("Index");
            }


            return View(model);
        }

        public async Task<IActionResult> Self1()
        {
            var _dbContext = new cyberContext();
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
            ViewData["IndustryType"] = ViewBag.Industry;
            //var industries = await ipos.GetIndustry();
            //ViewBag.Industry = new SelectList(industries, "Id", "Name");
            //ViewData["BrandType"] = ViewBag.Industry;
            //var industries = await ipos.GetAllIndustries();
            //var industryList = industries.Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name });
            //ViewData["IndustryType"] = industryList;


            var model = new ProspectSelfViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Self1(ProspectSelfViewModel model)
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

                //var pro = new ProsProd
                //{
                //    ProsId=prop.ProsId,
                //    ProductId=prop.ProductId,
                //    Discount=prop.Discount,
                //    EQty=prop.EQty,
                //    UPrice=prop.UPrice,
                //    STotal=prop.STotal,
                //    AddedOn=DateTime.Now
                //    //Name = model.Name,
                //    //IndustryType = model.IndustryType,
                //    //CountryName = model.CountryName,
                //    //State = model.State,
                //    //City = model.City,
                //    //Zip = model.Zip,
                //    //Address1 = model.Address1,
                //    //Address2 = model.Address2,
                //    //Email = model.Email,
                //    //CPerson = model.CPerson,
                //    //Number = model.Number,
                //    //Website = model.Website,
                //    //Gst = model.Gst,
                //    //Source = model.Source,
                //    //LeadType = model.LeadType,
                //    //Status = model.Status,
                //    //Terms = model.Terms,
                //    //Logo = "null"

                //};
                await ipos.Createselfprospect(pros);
                //await ipos.CreateProductprospect(pro);
                return RedirectToAction("Index");
            }


            return View(model);
        }


        public async Task<IActionResult> ResellerCreate()
        {
            var _dbContext = new cyberContext();


            var use = await ipos.GetAllusers();
            ViewBag.User = new SelectList(use, "UserId", "UserFname");
            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name");
            ViewData["Country"] = ViewBag.Countries;
            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name");
            ViewData["State"] = ViewBag.States;
            var city = await ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name");
            ViewData["City"] = ViewBag.Cities;
            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["BrandType"] = ViewBag.Industry;


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


        public IActionResult ImportExcel(IFormFile file)
        {
            var contacts = new Prospect();

            if (file != null && file.Length > 0)
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var pros = new Prospect
                        {
                            Name = worksheet.Cells[row, 1].Value?.ToString(),
                            IndustryType = worksheet.Cells[row, 2].Value?.ToString(),
                            CountryName = worksheet.Cells[row, 3].Value?.ToString(),
                            State = worksheet.Cells[row, 4].Value?.ToString(),
                            City = worksheet.Cells[row, 5].Value?.ToString(),
                            Zip = worksheet.Cells[row, 6].Value?.ToString(),
                            Address1 = worksheet.Cells[row, 7].Value?.ToString(),
                            Address2 = worksheet.Cells[row, 8].Value?.ToString(),
                            Email = worksheet.Cells[row, 9].Value?.ToString(),
                            CPerson = worksheet.Cells[row, 10].Value?.ToString(),
                            AddedOn = DateTime.TryParse(worksheet.Cells[row, 11].Value?.ToString(), out var addedOn) ? addedOn : DateTime.MinValue,
                            AddedBy = worksheet.Cells[row, 12].Value?.ToString(),
                            Status = Convert.ToInt32(worksheet.Cells[row, 13].Value?.ToString())
                            // ... your existing code to populate the product object ...
                        };

                        // contacts.Add(product);
                        context.Prospect.Add(pros);

                    }

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        //[HttpGet]
        //public IActionResult ExportToExcel()
        //{

            //var data = ipos.GetMyDatasProspects();
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //using (var package = new ExcelPackage())
            //{
            //    var worksheet = package.Workbook.Worksheets.Add("My Data");

            //    worksheet.Cells[1, 1].Value = "Name";
            //    worksheet.Cells[1, 2].Value = "IndustryType";
            //    worksheet.Cells[1, 3].Value = "CountryName";
            //    worksheet.Cells[1, 4].Value = "State";
            //    worksheet.Cells[1, 5].Value = "City";
            //    worksheet.Cells[1, 6].Value = "Zip";
            //    worksheet.Cells[1, 7].Value = "Address1";
            //    worksheet.Cells[1, 8].Value = "Address2";
            //    worksheet.Cells[1, 9].Value = "Email";
            //    worksheet.Cells[1, 10].Value = "CPerson";
            //    worksheet.Cells[1, 11].Value = "AddedOn";
            //    worksheet.Cells[1, 12].Value = "AddedBy";
            //    worksheet.Cells[1, 13].Value = "Status";
            //    var row = 2;
            //    foreach (var item in data)
            //    {
            //        worksheet.Cells[row, 1].Value = item.Name;
            //        worksheet.Cells[row, 2].Value = item.IndustryType;
            //        worksheet.Cells[row, 3].Value = item.CountryName;
            //        worksheet.Cells[row, 4].Value = item.State;
            //        worksheet.Cells[row, 5].Value = item.City;
            //        worksheet.Cells[row, 6].Value = item.Zip;
            //        worksheet.Cells[row, 7].Value = item.Address1;
            //        worksheet.Cells[row, 8].Value = item.Address2;
            //        worksheet.Cells[row, 9].Value = item.Email;
            //        worksheet.Cells[row, 10].Value = item.CPerson;
            //        worksheet.Cells[row, 11].Value = item.AddedOn;
            //        worksheet.Cells[row, 12].Value = item.AddedBy;
            //        worksheet.Cells[row, 13].Value = item.Status;
            //        row++;
            //    }
            //    var stream = new MemoryStream();
            //    package.SaveAs(stream);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "My Data.xls");
            //}


            //var data = ipos.GetMyData();
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //using (var package = new ExcelPackage())
            //{
            //    var worksheet = package.Workbook.Worksheets.Add("My Data");

            //    // Write the column headers
            //    worksheet.Cells[1, 1].Value = "Column 1";
            //    worksheet.Cells[1, 2].Value = "Column 2";
            //    worksheet.Cells[1, 3].Value = "Column 3";

            //    // Write the data
            //    var row = 2;
            //    foreach (var item in data)
            //    {
            //        worksheet.Cells[row, 1].Value = item.Name;
            //        worksheet.Cells[row, 2].Value = item.Number;
            //        worksheet.Cells[row, 3].Value = item.Email;
            //        row++;
            //    }

            //    // Create a memory stream and save the package to it
            //    var stream = new MemoryStream();
            //    package.SaveAs(stream);

            //    // Set the position of the stream back to the beginning
            //    stream.Seek(0, SeekOrigin.Begin);

            //    // Return the Excel file as a file download
            //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "My Data.xls");
            //}
        }
    
}