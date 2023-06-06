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
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace DMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private ILogin _ILogin;
        private IPostRepository _ipos;
        private readonly cyberContext _context;
        public LoginController(cyberContext cyberContext, IPostRepository ipos, IWebHostEnvironment webHostEnvironment)
        {
            _ipos = ipos;
            _context = cyberContext;
            _webHostEnvironment = webHostEnvironment;

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

            var user = _ILogin.ValidateUser(UserName, Password);

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
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DistributorViewmodel viewModel, IFormFile Taxdocumebt, IFormFile companyDocument, IFormFile UserLogo)
        {

            // Check if the ViewModel data is valid
            try
            {
                if (ModelState.IsValid)
                {

                    string password = GenerateRandomPassword();

                    // Create instances of the models and map the ViewModel data
                    var user = new User
                    {
                        UserFname = viewModel.DistributorName,
                        UserMail = viewModel.Email,
                        Pwd = HashPassword(password),
                        UserNumber = viewModel.DistributorNumber,
                        //UserType = viewModel.DistributorType,
                        UserType = "R",


                        UserStatus = 2, // Assuming a default status value
                        AddedOn = DateTime.Now,
                        UserPic = "null",
                        UserPwd = "null",
                        Bank = "null",
                        Readed = 0,
                        AReaded = null
                    };
                    if (UserLogo != null && UserLogo.Length > 0)
                    {
                        var extension = Path.GetExtension(UserLogo.FileName);

                        // Generate a unique filename using a GUID
                        var filename = $"{Guid.NewGuid()}{extension}";

                        // Set the path to save the file
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents");
                        string filePath = Path.Combine(directoryPath, filename);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            UserLogo.CopyTo(stream);
                        }

                        // Set the brand logo path to the uploaded file
                        user.UserLogo = $"/Documents/{filename}";

                    }

                    var item = _context.User.ToList();
                    var lastitem = item.Last();
                  
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
                   


                    var distriDocu = new DistriDocu
                    {
                        DistriId = lastmodel.DisId + 1,
                        Business = viewModel.CompanyDocument,
                        Opened = 0, // Assuming a default value for opened field
                        Status = 1, // Assuming a default status value
                        Gst = viewModel.Taxdocumebt,
                        AddedOn = DateTime.Now,
                        AddedBy = "self" // Assuming a default value for the added by field
                    };
                   
                    if (Taxdocumebt != null && Taxdocumebt.Length > 0)
                    {
                        string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Document");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string taxDocumentPath = Path.Combine(directoryPath, newDistributorId + "_tax" + Path.GetExtension(Taxdocumebt.FileName));
                        using (var fileStream = new FileStream(taxDocumentPath, FileMode.Create))
                        {
                            await Taxdocumebt.CopyToAsync(fileStream);
                        }
                        distriDocu.Gst = $"/Document/{newDistributorId}_tax{Path.GetExtension(Taxdocumebt.FileName)}";
                    }

                    // Save companyDocument file
                    if (companyDocument != null && companyDocument.Length > 0)
                    {
                        string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Document");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string companyDocumentPath = Path.Combine(directoryPath, newDistributorId + "_company" + Path.GetExtension(companyDocument.FileName));
                        using (var fileStream = new FileStream(companyDocumentPath, FileMode.Create))
                        {
                            await companyDocument.CopyToAsync(fileStream);
                        }
                        distriDocu.Business = $"/Document/{newDistributorId}_company{Path.GetExtension(companyDocument.FileName)}";
                    }
                    _context.User.Add(user);


                    _context.Distributor.Add(distributor);
                    _context.DistriDocu.Add(distriDocu);
                    _context.SaveChanges();
                    string emailBody = $"Cyber Iron Dome-Reseller Registration<br />" +
     $"Dear {viewModel.DistributorName}<br /><br />" +
     $"Congratulations, you have been successfully registered as a Reseller in Cyber Iron Dome.<br />" +
     $"Below are the credentials to log in to the system:<br />" +
     $"URL: https://dms.cyberirondome.net<br />" +
     $"Username: {viewModel.Email}<br />" +
     $"Password: {password}<br /><br />" +
     $"Please note that our admin will verify the initial Documents shared and approve the same<br />" +
     $"Request you to check your email for further Updates<br />" +
     $"Regards,<br />Cyber Iron Demo<br />" +
     $"<img src=\"~/img/logo.png\">";







                    await SendEmail(viewModel.Email, "new Reseller Registration", emailBody);


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

        private Task SendEmail(string email, string v, string emailBody)
        {
            throw new NotImplementedException();
        }

        private string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();

            int length = 6; // Set the length to 6 characters

            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }



        // Helper method to hash the password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private async Task SendEmail(string fromEmail, string fromName, string recipientEmail, string subject, string body, string ccEmail)
        {
            // Configure the SMTP client
            var smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dms.no-reply@cybrilliance.com", "Yar17157"),
                EnableSsl = true
            };

            // Create the email message
            var message = new MailMessage
            {
                //From = new MailAddress("dms.no-reply@cybrilliance.com"),
                From = new MailAddress(fromEmail, fromName),

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            // Add the recipient's email address
            message.To.Add(recipientEmail);
            if (!string.IsNullOrEmpty(ccEmail))
            {
                message.CC.Add(ccEmail);
            }

            // Send the email
            await smtpClient.SendMailAsync(message);
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


       

    }
}