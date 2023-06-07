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
    public class ProductController : Controller
    {
        IPostRepository ipos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly cyberContext _context;



        public ProductController(IPostRepository _ipos, IWebHostEnvironment webHostEnvironment, cyberContext context)
        {
            ipos = _ipos;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 7)
        {
            var productinfo = await ipos.GetProductDetails();
            if (!string.IsNullOrEmpty(searchString))
            {
                productinfo = productinfo.Where(b => (b.ProductName != null && b.ProductName.Contains(searchString)));


            }
            switch (sortOrder)
            {
                case "code_desc":
                    productinfo = productinfo.OrderByDescending(b => b.ProductCode);
                    break;
                case "name":
                    productinfo = productinfo.OrderBy(b => b.ProductName);
                    break;
                case "name_desc":
                    productinfo = productinfo.OrderByDescending(b => b.ProductName);
                    break;
                default:
                    productinfo = productinfo.OrderBy(b => b.ProductCode);
                    break;
            }
            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);


            // pass the current page number and total number of pages to the view


            //return View(productinfo);
            var pagedData = await productinfo.ToPagedListAsync(page, pageSize);

            // Pass the paged data to the view
            return View(pagedData);



        }
        public async Task<IActionResult> Createproduct()
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
            var brand = await ipos.GetBrands();
            ViewBag.Brand = new SelectList(brand, "BrandId", "BrandName"); // create SelectList from Countries list
            ViewData["Brand"] = ViewBag.Brand;

            return View("Createproduct");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Createproduct(ProductViewModel model, IFormFile logo, List<IFormFile> Files, string fileName)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    ProductPrice = model.ProductPrice,
                    Variant = model.Variant,
                    FromNode = model.FromNode,
                    ToNode = model.ToNode,
                    ProductDesc = model.ProductDesc,
                    ProductImg = model.ProductImg,
                    Files = new List<ProductFile>()
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
                    product.ProductImg = $"/upload/{filename}";

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
                            product.Files.Add(new ProductFile
                            {
                                FileName = file.FileName, // Use the file name entered by the user
                                FileType = file.ContentType,
                                File = file.FileName
                            });
                        }
                    }
                }
                await ipos.AddProducts(product);
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var brand = await ipos.GetProductDetailsByID(id);
            if (brand == null)
            {
                return NotFound();
            }

            brand.Status = (brand.Status == 1) ? 0 : 1;
            await ipos.EditProductDetails(brand);

            TempData["Message"] = (brand.Status == 1) ? "Record activated successfully." : "Record deactivated successfully.";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await ipos.GetByproductId(id);
            if (product == null)
            {
                return NotFound();
            }
            var model = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                FromNode = product.FromNode,
                ProductPrice = product.ProductPrice,
                Variant = product.Variant,
                ToNode = product.ToNode,
                ProductDesc = product.ProductDesc,
                ProductImg = product.ProductImg
            };
            var brandFiles = await (from p in _context.ProductFile
                                    where p.ProductId == id
                                    select p).ToListAsync();

            model.Files = brandFiles.Select(f => new ProductfileViewModel
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
        public async Task<IActionResult> Edit(int id, ProductViewModel model, IFormFile logo, List<IFormFile> Files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await ipos.GetByproductId(model.ProductId);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.ProductCode = model.ProductCode;
                    product.ProductName = model.ProductName;
                    product.ProductPrice = model.ProductPrice;
                    product.Variant = model.Variant;
                    product.FromNode = model.FromNode;
                    product.ToNode = model.ToNode;
                    product.ProductDesc = model.ProductDesc;
                    product.ProductImg = model.ProductImg;
                    product.Files = new List<ProductFile>();
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

                        product.ProductImg = $"/uploads/{filename}";
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

                                product.Files.Add(new ProductFile
                                {
                                    FileName = file.FileName,
                                    FileType = file.ContentType,
                                    File = file.FileName
                                });
                            }
                        }
                    }

                    await ipos.EditProductDetails(product);  // Assuming there's a method to update the brand in the repository

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
        //    var brand = await ipos.GetProductDetailsByID(id.Value);
        //    if (brand == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(brand);
        //}

        //// POST: Brand/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await ipos.EditProductDetails(product);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await ipos.GetProductDetailsByID(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var brand = await ipos.GetProductDetailsByID(id);
                if (brand == null)
                {
                    return NotFound();
                }

                await ipos.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
                // Handle exception and return error view
            }
        }
        [HttpGet]
        public IActionResult ExportToExcel()
        {
            var data = ipos.GetMyDatas();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("My Data");

                // Write the column headers
                worksheet.Cells[1, 1].Value = "ProductCode";
                worksheet.Cells[1, 2].Value = "Brand";
                worksheet.Cells[1, 3].Value = "Model";
                worksheet.Cells[1, 4].Value = "Variant";
                worksheet.Cells[1, 5].Value = "ProductName";
                worksheet.Cells[1, 6].Value = "ProductDesc";
                worksheet.Cells[1, 7].Value = "ProductImg";
                worksheet.Cells[1, 8].Value = "ProductPrice";
                worksheet.Cells[1, 9].Value = "FromNode";
                worksheet.Cells[1, 10].Value = "ToNode";
                worksheet.Cells[1, 11].Value = "AddedOn";
                worksheet.Cells[1, 12].Value = "AddedBy";
                worksheet.Cells[1, 13].Value = "Status";


                // Write the data
                var row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1].Value = item.ProductCode;
                    worksheet.Cells[row, 2].Value = item.Brand;
                    worksheet.Cells[row, 3].Value = item.Model;
                    worksheet.Cells[row, 4].Value = item.Variant;
                    worksheet.Cells[row, 5].Value = item.ProductName;
                    worksheet.Cells[row, 6].Value = item.ProductDesc;
                    worksheet.Cells[row, 7].Value = item.ProductImg;
                    worksheet.Cells[row, 8].Value = item.ProductPrice;
                    worksheet.Cells[row, 9].Value = item.FromNode;
                    worksheet.Cells[row, 10].Value = item.ToNode;
                    worksheet.Cells[row, 11].Value = item.AddedOn;
                    worksheet.Cells[row, 12].Value = item.AddedBy;
                    worksheet.Cells[row, 13].Value = item.Status;



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



        public IActionResult ImportExcel(IFormFile file)
        {
            var contacts = new Product();

            if (file != null && file.Length > 0)
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var product = new Product
                        {
                            ProductCode = worksheet.Cells[row, 1].Value?.ToString(),
                            Brand = Convert.ToInt32(worksheet.Cells[row, 2].Value?.ToString()),
                            Model = Convert.ToInt32(worksheet.Cells[row, 3].Value?.ToString()),
                            Variant = worksheet.Cells[row, 4].Value?.ToString(),
                            ProductName = worksheet.Cells[row, 5].Value?.ToString(),
                            ProductDesc = worksheet.Cells[row, 6].Value?.ToString(),
                            ProductImg = worksheet.Cells[row, 7].Value?.ToString(),
                            ProductPrice = Convert.ToInt32(worksheet.Cells[row, 8].Value?.ToString()),
                            FromNode = Convert.ToInt32(worksheet.Cells[row, 9].Value?.ToString()),
                            ToNode = Convert.ToInt32(worksheet.Cells[row, 10].Value?.ToString()),
                            AddedOn = DateTime.TryParse(worksheet.Cells[row, 11].Value?.ToString(), out var addedOn) ? addedOn : DateTime.MinValue,
                            AddedBy = worksheet.Cells[row, 12].Value?.ToString(),
                            Status = Convert.ToInt32(worksheet.Cells[row, 13].Value?.ToString())
                            // ... your existing code to populate the product object ...
                        };

                        // contacts.Add(product);
                        _context.Product.Add(product);

                    }

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}