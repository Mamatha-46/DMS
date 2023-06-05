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
    public class CompanyController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        cyberContext _context;

        public CompanyController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment, cyberContext context)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
            _context = context;

        }
        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 7)
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
            var users = await ipos.GetAllusers();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["BrandType"] = ViewBag.Industry;
            var model = new brandviewmodel();




            var brandinfo = await ipos.GetAllCompanies();

          
            if (!string.IsNullOrEmpty(searchString))
            {
                brandinfo = brandinfo.Where(b =>
                    b != null &&
                    (b.BrandName != null && b.BrandName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.BrandName).ToList();
                    break;
                case "type":
                    brandinfo = brandinfo.OrderBy(b => b.BrandType).ToList();
                    break;
                case "type_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.BrandType).ToList();
                    break;
                case "country":
                    brandinfo = brandinfo.OrderBy(b => b.CountryName).ToList();
                    break;
                case "country_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.CountryName).ToList();
                    break;
                case "state":
                    brandinfo = brandinfo.OrderBy(b => b.StateName).ToList();
                    break;
                case "state_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.StateName).ToList();
                    break;
                case "city":
                    brandinfo = brandinfo.OrderBy(b => b.City);
                    break;
                case "city_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.City);
                    break;
                case "zipcode":
                    brandinfo = brandinfo.OrderBy(b => b.Zipcode);
                    break;
                case "zipcode_desc":
                    brandinfo = brandinfo.OrderByDescending(b => b.Zipcode);
                    break;
                default:
                    brandinfo = brandinfo.OrderBy(b => b.BrandName);
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

        public async Task<IActionResult> Createcompany()
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
            var users = await ipos.GetAllusers();
            ViewBag.Industry = new SelectList(industries, "Id", "Name");
            ViewData["BrandType"] = ViewBag.Industry;
            var model = new brandviewmodel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createcompany(brandviewmodel model, IFormFile logo, List<IFormFile> Files, string fileName)
        {
            if (ModelState.IsValid)
            {
                var brand = new Brand
                {
                    BrandName = model.BrandName,
                    BrandType = model.BrandType,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    Zipcode = model.Zipcode,
                    Province = model.Province,
                    AddedBy = 0,
                    Files = new List<BrandFile>()
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
                    brand.Logo = $"/upload/{filename}";

                }
                // Check if any files were uploaded
                if (Files != null && Files.Count > 0)
                {
                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            // Save the file to disk
                            var extension = Path.GetExtension(file.FileName);

                            var filename = $"{Guid.NewGuid()}{extension}";
                            var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploading");

                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            var path = Path.Combine(directoryPath, filename);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            //  var fileName = Path.GetFileName(file.FileName); // Extract file name without path
                            brand.Files.Add(new BrandFile
                            {
                                FileName = file.FileName, // Use the file name entered by the user
                                FileType = file.ContentType,
                                File = file.FileName
                            });
                            //var customFileName = model.FileName; // Assuming the custom file name is stored in the 'FileName' property of the 'model' object

                            //brand.Files.Add(new BrandFile
                            //{
                            //    model.FileName = fileName,
                            //    FileType = file.ContentType,
                            //    File = file.FileName
                            //});
                            //model.FileName = file.FileName;
                            //var brandFile = new BrandFile
                            //{
                            //    FileName = fileName, // Use the filename entered by the user
                            //    FileType = file.ContentType,
                            //    File = fileName // Save the generated filename to the database
                            //};

                            //brand.Files.Add(brandFile);

                        }
                    }
                }
                //else if (model.File != null) // Assuming the single file is stored in the 'File' property of the 'model' object
                //{
                //    var file = model.File;

                //    if (file.Length > 0)
                //    {
                //        // Save the file to disk
                //        var extension = Path.GetExtension(file.FileName);
                //        var filename = $"{Guid.NewGuid()}{extension}";
                //        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploaded");

                //        if (!Directory.Exists(directoryPath))
                //        {
                //            Directory.CreateDirectory(directoryPath);
                //        }

                //        var path = Path.Combine(directoryPath, filename);

                //        using (var stream = new FileStream(path, FileMode.Create))
                //        {
                //            await file.CopyToAsync(stream);
                //        }

                //        brand.Files.Add(new BrandFile
                //        {
                //            FileName = file.FileName,
                //            FileType = file.ContentType,
                //            File = $"/uploaded/{filename}"
                //        });
                //    }
                //}
                await ipos.Createcompany(brand);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        private async Task<byte[]> ReadFileContentAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //public async Task<IActionResult> Createcompany(brandviewmodel model, IFormFile logo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var brand = new Brand
        //        {

        //            BrandName = model.BrandName,
        //            BrandType=model.BrandType,
        //            Address1 = model.Address1,
        //            Address2 = model.Address2,
        //            Country = model.Country,
        //            State = model.State,
        //            City = model.City,
        //            Zipcode = model.Zipcode,
        //            Province = model.Province,
        //            Material = model.Material,
        //        };

        //        if (logo != null && logo.Length > 0)
        //        {
        //            // Get the file extension
        //            var extension = Path.GetExtension(logo.FileName);

        //            // Genera.
        //            // te a unique filename using a GUID
        //            var filename = $"{Guid.NewGuid()}{extension}";

        //            // Set the path to save the file
        //            var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", filename);

        //            // Save the file to disk
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await logo.CopyToAsync(stream);
        //            }

        //            // Set the brand logo path to the uploaded file
        //            brand.Logo = $"/uploads/{filename}";
        //        }
        //        await Task.Delay(1000);
        //        await ipos.Createcompany(brand);
        //        return RedirectToAction("Index");
        //    }


        //    return View(model);
        //}
        public async Task<IActionResult> UploadFile2(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("uploads", file.FileName); // Path to save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Optionally, you can save the file path to a database or perform other operations
                return RedirectToAction("Index"); // Redirect to another page
            }
            return RedirectToAction("error", "home");

        }
        [HttpPost]
        public async Task<ProductFile> SavePath(ProductFile product)
        {
            var file = Request.Form.Files[0];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            product.File = file.FileName;
            _context.ProductFile.Add(product);
            await _context.SaveChangesAsync();
            await Task.Delay(2000);
            return product;
        }
        //public async Task<ActionResult> Call2Methods(brandviewmodel model, IFormFile logo, IFormFile file)
        //{
        //    await Createcompany(model, logo);
        //    await UploadFile2(file);
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await ipos.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }

            var countr = await ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name", brand.Country);
            ViewData["Country"] = ViewBag.Countries;

            var state = await ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name", brand.State);
            ViewData["State"] = ViewBag.States;

            var city = await ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name", brand.City);
            ViewData["City"] = ViewBag.Cities;

            var industries = await ipos.GetIndustry();
            ViewBag.Industry = new SelectList(industries, "Id", "Name", brand.BrandType);
            ViewData["BrandType"] = ViewBag.Industry;

            var model = new brandviewmodel
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                BrandType = brand.BrandType,
                Address1 = brand.Address1,
                Address2 = brand.Address2,
                Country = brand.Country,
                State = brand.State,
                City = brand.City,
                Zipcode = brand.Zipcode,
                Province = brand.Province,
                Material = brand.Material,
                Logo = brand.Logo
            };
            var brandFiles = await (from p in _context.BrandFile
                                    where p.BrandId == id
                                    select p).ToListAsync();

            model.Files = brandFiles.Select(f => new BrandFileViewModel
            {
                Id = f.Id,
                FileName = f.FileName,
                FileType = f.FileType,
                File = f.File
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, brandviewmodel model, IFormFile logo, List<IFormFile> Files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var brand = await ipos.GetById(model.BrandId);
                    if (brand == null)
                    {
                        return NotFound();
                    }


                    brand.BrandName = model.BrandName;
                    brand.BrandType = model.BrandType;
                    brand.Address1 = model.Address1;
                    brand.Address2 = model.Address2;
                    brand.Country = model.Country;
                    brand.State = model.State;
                    brand.City = model.City;
                    brand.Zipcode = model.Zipcode;
                    brand.Province = model.Province;
                    brand.AddedBy = 0;
                    brand.Files = new List<BrandFile>();

                    // Update the brand logo if a new logo is provided
                    if (logo != null && logo.Length > 0)
                    {
                        var extension = Path.GetExtension(logo.FileName);
                        var filename = $"{Guid.NewGuid()}{extension}";
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        var filePath = Path.Combine(directoryPath, filename);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await logo.CopyToAsync(stream);
                        }

                        brand.Logo = $"/uploads/{filename}";
                    }

                    // Check if any files were uploaded
                    if (Files != null && Files.Count > 0)
                    {
                        foreach (var file in Files)
                        {
                            if (file.Length > 0)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                var filename = $"{Guid.NewGuid()}{extension}";
                                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploading");

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                var path = Path.Combine(directoryPath, filename);

                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                brand.Files.Add(new BrandFile
                                {
                                    FileName = file.FileName,
                                    FileType = file.ContentType,
                                    File = file.FileName
                                });
                            }
                        }
                    }

                    await ipos.UpdateCompany(brand);  // Assuming there's a method to update the brand in the repository

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Handle any exceptions that occur during the update process
                    // You can log the exception or return an appropriate error message
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            // If the ModelState is not valid, return the view with the model
            return View(model);
        }



        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var brand = await ipos.GetById(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(brand);
        //}

        //// POST: Brand/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Brand brand)
        //{
        //    if (id != brand.BrandId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ipos.UpdateCompany(brand);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(brand);
        //}

        public async Task<IActionResult> Details(int? id)
        {
            var brand = await ipos.GetById(id.Value);

            if (id == null)
            {
                return NotFound();
            }

            var model = new brandviewmodel
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                BrandType = brand.BrandType,
                Address1 = brand.Address1,
                Address2 = brand.Address2,
                Country = brand.Country,
                State = brand.State,
                City = brand.City,
                Zipcode = brand.Zipcode,
                Province = brand.Province,
                Material = brand.Material,
                Logo = brand.Logo
            };
            var brandFiles = await (from p in _context.BrandFile
                                    where p.BrandId == id
                                    select p).ToListAsync();

            model.Files = brandFiles.Select(f => new BrandFileViewModel
            {
                Id = f.Id,
                FileName = f.FileName,
                FileType = f.FileType,
                File = f.File
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await ipos.GetById(id);
                if (brand == null)
                {
                    return NotFound();
                }

                await ipos.DeleteCompany(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
                // Handle exception and return error view
            }
        }
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    var brand = await ipos.GetById(id);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }

        //    brand.Status = (brand.Status == 0) ? 0 : 1;
        //    await ipos.UpdateCompany(brand);

        //    TempData["Message"] = (brand.Status == 0) ? "Record deactivated successfully." : "Record activated successfully.";

        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var brand = await ipos.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }
            brand.Status = (brand.Status == 1) ? 0 : 1;
            await ipos.UpdateCompany(brand);

            TempData["Message"] = (brand.Status == 0) ? "Record activated successfully." : "Record deactivated successfully.";
            return RedirectToAction("Index");
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

        //public async Task<BrandFile> UploadFile(IFormFile file, Brand brand)
        //{
        //    var fileName = Path.GetFileName(file.FileName);
        //    var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploads", fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    // create new BrandFile object and set its properties
        //    var brandFile = new BrandFile
        //    {
        //        FileName = fileName,
        //        FilePath = filePath,
        //        Brand = brand
        //    };

        //    // save the BrandFile object to the database
        //    _dbContext.BrandFiles.Add(brandFile);
        //    await _dbContext.SaveChangesAsync();

        //    return brandFile;
        //}

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

    }
}