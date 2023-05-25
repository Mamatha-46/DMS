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
    public class clientController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly cyberContext context;


        public clientController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment, cyberContext _context)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
            context = _context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 7)
        {
            var brandinfo = await ipos.GetProspectDetails();
            //return View(brandinfo);
            if (!string.IsNullOrEmpty(searchString))
            {
                //brandinfo = brandinfo.Where(b =>
                //    b != null &&
                //    (b.UserFname != null && b.UserFname.Contains(searchString)) ||
                //    (b.Name != null && b.Name.Contains(searchString)) ||
                //    (b.CPerson != null && b.CPerson.Contains(searchString)) ||
                //      (b.Email != null && b.Email.Contains(searchString)) ||
                //    (b.CountryName != null && b.CountryName.Contains(searchString))


                brandinfo = brandinfo.Where(b => b != null &&
                        (b.UserFname != null && b.UserFname.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (b.Name != null && b.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (b.CPerson != null && b.CPerson.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (b.Email != null && b.Email.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (b.CountryName != null && b.CountryName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0));

                //(b.Number != null && b.Number.ToString() != null && b.Number.ToString().Contains(searchString)) ||

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
        }

            public ActionResult Import(IFormFile excelFile)
            {
                //var prosdoc = new List<ProspectDoc>();

                if (excelFile != null && excelFile.Length > 0)
                {
                    using (var package = new ExcelPackage(excelFile.OpenReadStream()))
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {

                            var prospectdoc = new ProspectDoc
                            {
                                ProspectName = worksheet.Cells[row, 1].Value?.ToString(),
                                IndustryType = worksheet.Cells[row, 2].Value?.ToString(),
                                Country = worksheet.Cells[row, 3].Value?.ToString(),
                                State = worksheet.Cells[row, 4].Value?.ToString(),
                                City = worksheet.Cells[row, 5].Value?.ToString(),
                                Zipcode = worksheet.Cells[row, 6].Value?.ToString(),
                                Address1 = worksheet.Cells[row, 7].Value?.ToString(),
                                Address2 = worksheet.Cells[row, 8].Value?.ToString(),
                                EmailAddress = worksheet.Cells[row, 9].Value?.ToString(),
                                ContactPerson = worksheet.Cells[row, 10].Value?.ToString(),
                                Phone = Convert.ToInt64(worksheet.Cells[row, 11].Value?.ToString()),
                                Website = worksheet.Cells[row, 12].Value?.ToString(),
                                Gst = worksheet.Cells[row, 13].Value?.ToString(),
                                Source = worksheet.Cells[row, 14].Value?.ToString(),
                                Tax = Convert.ToInt32(worksheet.Cells[row, 15].Value?.ToString()),
                                LeadType = worksheet.Cells[row, 16].Value?.ToString()

                            };

                            var country = new Countries
                            {
                                Name = prospectdoc.Country
                            };

                            context.Countries.Add(country);

                            var state = new States
                            {
                                Name = prospectdoc.State
                            };

                            context.States.Add(state);

                            var city = new Cities
                            {
                                Name = prospectdoc.City
                            };

                            context.Cities.Add(city);

                            var industry = new Industry
                            {
                                Name = prospectdoc.IndustryType
                            };

                            context.Industry.Add(industry);

                            var prospect = new Prospect
                            {
                                Name = prospectdoc.ProspectName,
                                CountryId = country.Id,  // Assuming 'Id' is the primary key of the 'Countries' table
                                State = state.Id.ToString(),  // Assuming 'Id' is the primary key of the 'States' table
                                City = city.Id.ToString(),  // Assuming 'Id' is the primary key of the 'Cities' table
                                IndustryType = industry.Id.ToString(),
                                Zip = prospectdoc.Zipcode,
                                Address1 = prospectdoc.Address1,
                                Address2 = prospectdoc.Address2,
                                Email = prospectdoc.EmailAddress,
                                CPerson = prospectdoc.ContactPerson,
                                Number = prospectdoc.Phone,
                                Website = prospectdoc.Website,
                                Gst = prospectdoc.Gst,
                                Source = prospectdoc.Source,
                                LeadType = prospectdoc.LeadType
                            };

                            context.Prospect.Add(prospect);

                        }

                        context.SaveChanges();
                        return RedirectToAction("Index");

                    }
                }

                return View();
            }



            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);

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
            var products = await ipos.GetProducts();
            ViewBag.Product = new SelectList(products, "ProductId", "ProductName");
            ViewData["ProductId"] = ViewBag.Product;

            var model = new prospectviewmodel();
            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> CreateResellerProspect(prospectviewmodel model, IFormFile logo)
        {
            if (ModelState.IsValid)
            {
                Prospect prospect = new Prospect
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
                    //Status=model.Status
                };
                if (logo != null && logo.Length > 0)
                {
                    // Get the file extension
                    var extension = Path.GetExtension(logo.FileName);

                    // Generate a unique filename using a GUID
                    var filename = $"{Guid.NewGuid()}{extension}";

                    // Set the path to save the file
                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                    string filePath = Path.Combine(directoryPath, filename);


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }

                    // Set the brand logo path to the uploaded file
                    prospect.Logo = $"/upload/{filename}";

                }
                context.Prospect.Add(prospect);
                await context.SaveChangesAsync();




                ProsProd prosProd = new ProsProd
                {
                    ProsId = prospect.Id,
                    ProductId = model.ProductId,
                    Discount = model.Discount,
                    EQty = model.EQty,
                    UPrice = model.UPrice,
                    STotal = model.STotal
                };

                // Save the product to the database
                context.ProsProd.Add(prosProd);


                // Save the changes to the database
                await context.SaveChangesAsync();


                ProsProdLoop prosProdLoop = new ProsProdLoop
                {
                    ProsProdId = prospect.Id,
                    Stage = model.Stage,
                    Date = DateTime.Now,
                    Comments = model.Comments

                };

                context.ProsProdLoop.Add(prosProdLoop);

                await context.SaveChangesAsync();


                return RedirectToAction("CreateResellerProspect");

            }
            return View(model);

        }
        public async Task<IActionResult> EditResellerProspect(int id)
        {
            var prospect = await context.Prospect.FindAsync(id);

            if (prospect == null)
            {
                return NotFound();
            }

            var model = new prospectviewmodel
            {
                // Populate the view model with prospect data
                Id = prospect.Id,
                AddedId = prospect.AddedId,
                Name = prospect.Name,
                IndustryType = prospect.IndustryType,
                CountryName = prospect.CountryName,
                State = prospect.State,
                City = prospect.City,
                Zip = prospect.Zip,
                Address1 = prospect.Address1,
                Address2 = prospect.Address2,
                Email = prospect.Email,
                CPerson = prospect.CPerson,
                Number = prospect.Number,
                Website = prospect.Website,
                Gst = prospect.Gst,
                Source = prospect.Source,
                LeadType = prospect.LeadType,
                Logo = prospect.Logo // Pass the logo path to the view model
            };

            var countries = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            var states = await ipos.GetStates();
            ViewBag.States = new SelectList(states, "Id", "Name");

            var cities = await ipos.GetCities();
            ViewBag.Cities = new SelectList(cities, "Id", "Name");

            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");

            var users = await ipos.GetAllusers();
            ViewBag.User = new SelectList(users, "UserId", "UserFname");

            var products = await ipos.GetProducts();
            ViewBag.Product = new SelectList(products, "ProductId", "ProductName");

            // Retrieve the ProsProd details for the prospect
            //var prosProd = await context.ProsProd.Where(pp => pp.ProsId == id).FirstOrDefaultAsync();
            var pros = await (from p in context.ProsProd
                              where p.ProsId == id
                              select p).FirstOrDefaultAsync();
            if (pros != null)
            {
                // Populate the view model with ProsProd data
                model.ProductId = pros.ProductId;
                model.Discount = pros.Discount;
                model.EQty = pros.EQty;
                model.UPrice = pros.UPrice;
                model.STotal = pros.STotal;
            }

            // Retrieve the ProsProdLoop details for the prospect
            var prosProdLoop = await context.ProsProdLoop.Where(ppl => ppl.ProsProdId == id).FirstOrDefaultAsync();
            if (prosProdLoop != null)
            {
                // Populate the view model with ProsProdLoop data
                model.Stage = prosProdLoop.Stage;
                model.Date = prosProdLoop.Date;
                model.Comments = prosProdLoop.Comments;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditResellerProspect(prospectviewmodel model, IFormFile logo)
        {
            if (ModelState.IsValid)
            {
                var prospect = await context.Prospect.FindAsync(model.Id);

                if (prospect == null)
                {
                    return NotFound();
                }

                prospect.AddedId = model.AddedId;
                prospect.Name = model.Name;
                prospect.IndustryType = model.IndustryType;
                prospect.CountryName = model.CountryName;
                prospect.State = model.State;
                prospect.City = model.City;
                prospect.Zip = model.Zip;
                prospect.Address1 = model.Address1;
                prospect.Address2 = model.Address2;
                prospect.Email = model.Email;
                prospect.CPerson = model.CPerson;
                prospect.Number = model.Number;
                prospect.Website = model.Website;
                prospect.Gst = model.Gst;
                prospect.Source = model.Source;
                prospect.LeadType = model.LeadType;

                if (logo != null && logo.Length > 0)
                {
                    // Get the file extension
                    var extension = Path.GetExtension(logo.FileName);

                    // Generate a unique filename using a GUID
                    var filename = $"{Guid.NewGuid()}{extension}";

                    // Set the path to save the file
                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    string filePath = Path.Combine(directoryPath, filename);


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }

                    // Set the brand logo path to the uploaded file
                    prospect.Logo = $"/uploads/{filename}";

                }

                // Update the logo path in the prospect


                context.Prospect.Update(prospect);
                await context.SaveChangesAsync();

                var prosProd = await context.ProsProd.FirstOrDefaultAsync(pp => pp.ProsId == model.Id);
                if (prosProd != null)
                {
                    // Update the ProsProd details
                    prosProd.ProductId = model.ProductId;
                    prosProd.Discount = model.Discount;
                    prosProd.EQty = model.EQty;
                    prosProd.UPrice = model.UPrice;
                    prosProd.STotal = model.STotal;

                    context.ProsProd.Update(prosProd);
                }

                var prosProdLoop = await context.ProsProdLoop.FirstOrDefaultAsync(ppl => ppl.ProsProdId == model.Id);
                if (prosProdLoop != null)
                {
                    // Update the ProsProdLoop details
                    prosProdLoop.Stage = model.Stage;
                    prosProdLoop.Date = model.Date;
                    prosProdLoop.Comments = model.Comments;

                    context.ProsProdLoop.Update(prosProdLoop);
                }

                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
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
        //[HttpPost]
        //public async Task<IActionResult> CreateResellerProspect(Prospect prospect, IFormFile logo)
        //{

        //    //await ipos.insertprospects(prospect);
        //    // return RedirectToAction("Index");
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (logo != null && logo.Length > 0)
        //            {
        //                // Get the file extension
        //                var extension = Path.GetExtension(logo.FileName);

        //                // Generate a unique filename using a GUID
        //                var filename = $"{Guid.NewGuid()}{extension}";

        //                // Set the path to save the file
        //                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);

        //                // Save the file to disk
        //                using (var stream = new FileStream(path, FileMode.Create))
        //                {
        //                    await logo.CopyToAsync(stream);
        //                }

        //                // Set the brand logo path to the uploaded file
        //                prospect.Logo = $"/uploads/{filename}";

        //            }


        //            await ipos.insertprospects(prospect);
        //            return RedirectToAction("Index");
        //        }


        //        return View("createproduct");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }


        //}
        //[HttpPost]
        //public async Task<string> UploadFile(IFormFile file)
        //{
        //    var fileName = Path.GetFileName(file.FileName);
        //    var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return $"/uploads/{fileName}";
        //}



        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var prospect = await ipos.GetProspectDetailsByID(id.Value);
        //    if (prospect == null)
        //    {
        //        return NotFound();
        //    }
        //    var countr = await ipos.GetCountries();
        //    ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["CountryName"] = ViewBag.Countries;
        //    //var prospect = new Prospect();
        //    prospect.CountryId = countr.FirstOrDefault(c => c.Name == prospect.CountryName)?.Id;
        //    // set default country id
        //    prospect.CountryName = countr.FirstOrDefault()?.Name; // set default country name


        //    var state = await ipos.GetStates();
        //    ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["State"] = ViewBag.States;
        //    var city = await ipos.GetCities();

        //    ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
        //    ViewData["City"] = ViewBag.Cities;
        //    var industries = await ipos.GetIndustry();
        //    ViewBag.Industry = new SelectList(industries, "Id", "Name");
        //    ViewData["IndustryType"] = ViewBag.Industry;
        //    var Users = await ipos.GetAllusers();
        //    ViewBag.User = new SelectList(Users, "UserId", "UserFname");
        //    ViewData["AddedId"] = ViewBag.User;


        //    return View(prospect);
        //}

        //// POST: Brand/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Prospect prospect)
        //{
        //    if (id != prospect.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ipos.EditProspectDetails(prospect);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(prospect);
        //}

        [HttpGet]
        public IActionResult ExportToExcel()
        {
            var data = ipos.GetMyData();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("My Data");

                // Write the column headers
                worksheet.Cells[1, 1].Value = "Prospect Name";
                worksheet.Cells[1, 2].Value = "Industry Type";
                worksheet.Cells[1, 3].Value = "Country";
                worksheet.Cells[1, 4].Value = "State/ Region";
                worksheet.Cells[1, 5].Value = "City";
                worksheet.Cells[1, 6].Value = "Zip code";
                worksheet.Cells[1, 7].Value = "Address1";
                worksheet.Cells[1, 8].Value = "Address2";
                worksheet.Cells[1, 9].Value = "Email Address";
                worksheet.Cells[1, 10].Value = "Contact Person Name";
                worksheet.Cells[1, 11].Value = "Phone/ Mobile No.";
                worksheet.Cells[1, 12].Value = "Website";
                worksheet.Cells[1, 13].Value = "GST/ VAT No.";
                worksheet.Cells[1, 14].Value = "Source";
                worksheet.Cells[1, 15].Value = "Tax(%)";
                worksheet.Cells[1, 16].Value = "Lead Type";


                // Write the data
                var row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.ProspectName;
                    worksheet.Cells[row, 2].Value = item.IndustryType;
                    worksheet.Cells[row, 3].Value = item.Country;
                    worksheet.Cells[row, 4].Value = item.State;
                    worksheet.Cells[row, 5].Value = item.City;
                    worksheet.Cells[row, 6].Value = item.Zipcode;
                    worksheet.Cells[row, 7].Value = item.Address1;
                    worksheet.Cells[row, 8].Value = item.Address2;
                    worksheet.Cells[row, 9].Value = item.EmailAddress;
                    worksheet.Cells[row, 10].Value = item.ContactPerson;
                    worksheet.Cells[row, 11].Value = item.Phone;
                    worksheet.Cells[row, 12].Value = item.Website;
                    worksheet.Cells[row, 13].Value = item.Gst;
                    worksheet.Cells[row, 14].Value = item.Source;
                    worksheet.Cells[row, 15].Value = item.Tax;
                    worksheet.Cells[row, 16].Value = item.LeadType;


                    row++;
                }

                // Create a memory stream and save the package to it
                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Set the position of the stream back to the beginning
                stream.Seek(0, SeekOrigin.Begin);

                // Return the Excel file as a file download
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "My Data.xls");
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
            //List<ProspectEditView> prospecteditview = new List<ProspectEditView>();

            var result = (from p in context.Prospect
                          join pp in context.ProsProd on p.Id equals pp.ProsId
                          join ppl in context.ProsProdLoop on pp.ProsId equals ppl.ProsProdId
                          where p.Id == id
                          select new prospectviewmodel
                          {
                              ProductId = p.Id,
                              Discount = pp.Discount,
                              EQty = pp.EQty,
                              UPrice = pp.UPrice,
                              STotal = pp.STotal,
                              Stage = ppl.Stage,
                              Date = ppl.Date,
                              Comments = ppl.Comments
                          }).ToList();







            ViewBag.ProspectProducts = result;
            return View();

        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, prospectviewmodel model, IFormFile logo)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Prospect prospect = new Prospect
                {
                    AddedId = model.AddedId,
                    Name = model.Name,
                    //IndustryType = model.IndustryType,
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
                    //Status=model.Status
                };
                if (logo != null && logo.Length > 0)
                {
                    // Get the file extension
                    var extension = Path.GetExtension(logo.FileName);

                    // Generate a unique filename using a GUID
                    var filename = $"{Guid.NewGuid()}{extension}";

                    // Set the path to save the file
                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    string filePath = Path.Combine(directoryPath, filename);


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        logo.CopyTo(stream);
                    }

                    // Set the brand logo path to the uploaded file
                    prospect.Logo = $"/uploads/{filename}";

                }
                context.Prospect.Add(prospect);
                await context.SaveChangesAsync();




                ProsProd prosProd = new ProsProd
                {
                    ProsId = prospect.Id,
                    ProductId = model.ProductId,
                    Discount = model.Discount,
                    EQty = model.EQty,
                    UPrice = model.UPrice,
                    STotal = model.STotal
                };

                // Save the product to the database
                context.ProsProd.Add(prosProd);


                // Save the changes to the database
                await context.SaveChangesAsync();


                ProsProdLoop prosProdLoop = new ProsProdLoop
                {
                    ProsProdId = prospect.Id,
                    Stage = model.Stage,
                    Date = DateTime.Now,
                    Comments = model.Comments

                };

                context.ProsProdLoop.Add(prosProdLoop);

                await context.SaveChangesAsync();


                return RedirectToAction("Index");

            }
            return View(model);


        }
    }
}

