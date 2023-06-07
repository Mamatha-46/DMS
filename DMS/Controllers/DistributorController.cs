using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Repository;
using DMS.Models;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;

namespace DMS.Controllers
{
    public class DistributorController : Controller
    {
        private readonly IPostRepository _ipr;
        cyberContext _context;
        private IPostRepository _ipos;
        public DistributorController(IPostRepository ipr, cyberContext context, IPostRepository ipos)
        {
            _context = context;
            _ipr = ipr;
            _ipos = ipos;

        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pagesize = 5)
        {
            var countr = await _ipos.GetCountries();
            ViewBag.Countries = new SelectList(countr, "Id", "Name"); // create SelectList from Countries list
            ViewData["Country"] = ViewBag.Countries;
            var state = await _ipos.GetStates();
            ViewBag.States = new SelectList(state, "Id", "Name"); // create SelectList from Countries list
            ViewData["State"] = ViewBag.States;
            var city = await _ipos.GetCities();
            ViewBag.Cities = new SelectList(city, "Id", "Name"); // create SelectList from Countries list
            ViewData["City"] = ViewBag.Cities;




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


        //public void SendApprovalEmail(bool isApproved, string dcname, string remarks, string gst_remark, string email)
        //    {
        //        string subject = isApproved ? "Cyber Iron Dome - Reseller Initial Document - Approved" : "Cyber Iron Dome - Reseller Initial Document - Rejection";
        //        string mailMessage = "<html><body>";
        //        mailMessage += "<font size=2 face=Verdana color=#000080>";
        //        mailMessage += "<p align=left><b>Dear " + dcname + ", <br>";
        //        if (isApproved)
        //        {
        //            mailMessage += "We have reviewed your documents and submitted documents are approved by our Admin. Below is the Status of your Initial Documents. </b></p><br>";
        //            mailMessage += "<p align=left><b><u>Company Registration Document</u></b></p>\r\n";
        //            mailMessage += "<p align=left><b>Status : </b>&nbsp; Approved</p>\r\n";
        //            mailMessage += "<p align=left><b>Reason : </b>&nbsp; " + remarks + "</p><br>\r\n";
        //            mailMessage += "<p align=left><b><u>Tax Document:</u></b></p>\r\n";
        //            mailMessage += "<p align=left><b>Status : </b>&nbsp; Approved</p>\r\n";
        //            mailMessage += "<p align=left><b>Reason : </b>&nbsp;" + gst_remark + "</p><br>\r\n";
        //            mailMessage += "<p align=left><b></b>Admin will generate the agreement soon and will be shared via email.</p>";
        //            mailMessage += "<p>Request you to check your email for further updates.</p><br><br><br>";
        //        }
        //        else
        //        {
        //            mailMessage += "<p>We have reviewed your documents and observed that the below document(s) are not valid. Please find the below reason why it was rejected. Request you to update and resend the registered Document to us for further review.</b></p><br>";
        //            mailMessage += "<p align=left><b><u>Company Registration Document</u></b></p>\r\n";
        //            //mailMessage += "<p align=left><b>Status : </b>&nbsp; " + DistriDocu. + "\r\n";
        //            mailMessage += "<p align=left><b>Reason : </b>&nbsp; " + remarks + "</p><br>\r\n";
        //            mailMessage += "<p align=left><b><u>Tax Document:</u></b></p>\r\n";
        //            //mailMessage += "<p align=left><b>Status : </b>&nbsp; " + taxDocumentStatus + "</p>\r\n";
        //            mailMessage += "<p align=left><b>Reason : </b>&nbsp;" + gst_remark + "</p><br><br><br>";
        //            mailMessage += "<p align=left>Request you to check your email for further updates.</p>";
        //        }
        //        mailMessage += "<strong>Regards, <br>Cyber Iron Dome Team</strong><br>";
        //            mailMessage += "<img src='cid:logo' alt='logo' height='60' width='200' /><br><br>";
        //            mailMessage += "<strong>3630 Unit 5 Odessey Drive L5M-7N4 Mississauga Canada</strong><br>";
        //        mailMessage += "<strong>+1 905-330-1455, info@cyberirondome.com</strong>";
        //        mailMessage += "</font></body></html>";

        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.From = new MailAddress("dms.no-reply@cyberirondome.com");
        //            mail.To.Add("sudhakar@matayo-ai.com");
        //            mail.To.Add(email);
        //            mail.Subject = subject;
        //            mail.Body = mailMessage;
        //            mail.IsBodyHtml = true;

        //            using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
        //            {
        //                smtp.EnableSsl = true;
        //                smtp.UseDefaultCredentials = false;
        //                smtp.Credentials = new System.Net.NetworkCredential("dms.no-reply@cyberirondome.com", "Yar17157");

        //                smtp.Send(mail);
        //            }
        //        }
        //    }


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

        [HttpGet]
        public async Task<IActionResult> AgreementDetails1(int? DisID)
        {
            var products = await _ipos.GetProducts();
            ViewBag.Product = new SelectList(products, "ProductId", "ProductName");
            ViewData["product-id"] = ViewBag.Product;

            if (DisID == null)
            {
                return NotFound();
            }
            var reseller = await _ipr.GetResellersDetailsView(DisID.Value);

            if (reseller == null)
            {
                return NotFound();
            }
            return View(reseller);

        }

        [HttpPost]
        public async Task<IActionResult> Adduserrenewal(UserRenewal usr)
        {
            if (ModelState.IsValid)
            {
                var addedUserRenewal = await _ipr.Adduserrenewal(usr);

                if (addedUserRenewal != null)
                {
                    return View (addedUserRenewal);
                }
                return BadRequest("Failed to add user renewal.");
            }
            return BadRequest("Invalid model state.");
        }

        [HttpPost]
        public async Task<IActionResult> Addproductreseler(ProsProd pp)
        {


            if (ModelState.IsValid)
            {
                var Addproductreseler = await _ipr.Addproductreseler(pp);

                if (Addproductreseler != null)
                {
                    return Ok(Addproductreseler);
                }
                return BadRequest("Failed to add user renewal.");
            }
            return BadRequest("Invalid model state.");
        }


        [HttpGet]
        public async  Task<IActionResult> getproductbyId(int id)
        {
            var item = await _context.Product.FindAsync(id);
            return  (IActionResult)item;
         }


        [HttpPost]
        public async Task<IActionResult> updatedocstatus([FromBody] DistDocView dic)
        {
            var item = await _ipr.updatedocstatus(dic);
           

            int busStatus = Convert.ToInt32 (dic.Bus_Status);
            int taxStatus = Convert.ToInt32 ( dic.Tax_Status);
            string reBRemarks = dic.ReBRemarks;
            string reGRemarks = dic.ReGRemarks;

            string DistributorName = dic.DistributorName;
            string Email = dic.Email;
            if (busStatus == 1 && taxStatus == 1)
            {
                string subject = "Cyber Iron Dome - Reseller Initial Document - Approved";
                string mailMessage = $@"<html>
                        <body>
                            <font size=2 face=Verdana color=#000080>
                                <p align=left><b>Dear {DistributorName},</b></p>
                                <p align=left>We have reviewed your documents and submitted documents are approved by our Admin. Below is the status of your initial documents.</p>
                                <p align=left><b><u>Company Registration Document:</u></b></p>
                                <p align=left><b>Status: </b> Approved</p>
                                <p align=left><b>Reason: </b> {reBRemarks}</p>
                                <p align=left><b><u>Tax Document:</u></b></p>
                                <p align=left><b>Status: </b> Approved</p>
                                <p align=left><b>Reason: </b> {reGRemarks}</p>
                                <p align=left><b>Admin will generate the agreement soon and will be shared via email.</b></p>
                                <p>Request you to check your email for further updates.</p>
                                <br><br><br>
                                <strong>Regards,<br>Cyber Iron Dome Team</strong><br>
                                <img src='cid:logo' alt='logo' style='height:60px; width:200px;' /><br><br>
                                <strong>3630 Unit 5 Odessey Drive L5M-7N4 Mississauga Canada</strong><br>
                                <strong>+1 905-330-1455, info@cyberirondome.com</strong>
                            </font>
                        </body>
                    </html>";

                await SendEmail( Email,subject, mailMessage);
            }
            else if (busStatus == 0 && taxStatus == 0)
            {
                string subject = "Cyber Iron Dome - Reseller Initial Document - Rejection";
                string emailBody = $@"<html>
                        <body>
                            <font size=2 face=Verdana color=#000080>
                                <p align=left><b>Dear {DistributorName} ,</b></p>
                                <p align=left>We have reviewed your documents and observed that the below document(s) are not valid. Please find the reasons for rejection below. Request you to update and resend the registered documents to us for further review.</p>
                                <p align=left><b><u>Company Registration Document:</u></b></p>
                                <p align=left><b>Reason: </b> {reBRemarks}</p>
                                <p align=left><b><u>Tax Document:</u></b></p>
                                <p align=left><b>Reason: </b> {reGRemarks}</p>
                                <br><br>
                                <p align=left>Request you to check your email for further updates.</p>
                                <br>
                                <strong>Regards,<br>Cyber Iron Dome Team</strong>
                                <br>
                                <img src='cid:logo' style='height:60px; width:200px;' /><br><br>
                                <strong>3630 Unit 5 Odessey Drive L5M-7N4 Mississauga Canada</strong><br>
                                <strong>+1 905-330-1455, info@cyberirondome.com</strong>
                            </font>
                        </body>
                    </html>";

                await SendEmail(Email, subject, emailBody);
            }

            return Json(item);
        }

       
        private async Task SendEmail(string recipientEmail, string subject, string body)
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
                From = new MailAddress("dms.no-reply@cybrilliance.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            // Add the recipient's email address

            message.To.Add(recipientEmail);

            // Send the email

            await smtpClient.SendMailAsync(message);
        }
        

    }
}