using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Models;
using DMS.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using DMS.BAL;
using X.PagedList;



namespace DMS.Controllers
{
    public class PartnerController : Controller
    {

        private readonly IPostRepository ipos;
        private readonly ILogger<HomeController> _logger;
        private readonly cyberContext _context;

        public PartnerController(ILogger<HomeController> logger, cyberContext context, IPostRepository _ipos)
        {
            _context = context;
            _logger = logger;
            ipos = _ipos;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult PartnerDashboard()
        {
            int totalPartners = _context.User.Where(u => u.UserType == "R" && u.UserStatus == 1).Count();
            ViewBag.TotalPartners = totalPartners;
            var totalAgreementsSignedByPartner = _context.UserRenewal.Where(ur => ur.RSign == 1 && ur.CidRSign == 1)
                                                 .Select(ur => ur.UserId)
                                                 .Distinct()
                                                 .Count();
            ViewBag.TotalAgreementsSignedByPartner = totalAgreementsSignedByPartner;
            var totalAgreementsSignedByAdmin = _context.UserRenewal.Where(u => u.CompAgreement != null && u.CompAgreement2 != null)
                                               .Select(u => u.UserId)
                                               .Distinct()
                                               .Count();
            ViewBag.TotalAgreementsSignedByAdmin = totalAgreementsSignedByAdmin;
            var totalprospects = _context.Prospect.Where(p => p.ProsType == 0).Count();
            ViewBag.TotalProspects = totalprospects;
   
            var query = from prospect in _context.Prospect
                        join prosProd in _context.ProsProd on prospect.Id equals prosProd.ProsId
                        where prospect.ProsType == 0 &&
                         prosProd.Status == 0 && prosProd.FStage == "quoteG"
                        select new { Prospect = prospect, ProsProd = prosProd };

            var quotationShared = query.Count();
            ViewBag.QuotationShared = quotationShared;
            var trailLicenceShared = from prospect in _context.Prospect
                                     join prosProd in _context.ProsProd on prospect.Id equals prosProd.ProsId
                                     where prospect.ProsType == 0 &&
                                           prosProd.Status == 0 &&
                                           prosProd.FStage == "trail"
                                     select new { Prospect = prospect, ProsProd = prosProd };
            var rowCount = trailLicenceShared.Count();
            ViewBag.TrailLicenceShared = rowCount;
            var totalProspects = _context.Prospect.Where(prospect => prospect.ProsType == 1).Count();
            ViewBag.TotalPropspects = totalProspects;
            var clientsquotationShared = from prospect in _context.Prospect
                                         join prosProd in _context.ProsProd on prospect.Id equals prosProd.ProsId
                                         where prospect.ProsType == 1 &&
                                               prosProd.Status == 0 &&
                                               prosProd.FStage == "quoteG"
                                         select new { Prospect = prospect, ProsProd = prosProd };

            var rowCounts = clientsquotationShared.Count();
            ViewBag.ClientsquotationShared = rowCounts;
            var clienttrailLicenceShared = from prospect in _context.Prospect
                                           join prosProd in _context.ProsProd on prospect.Id equals prosProd.ProsId
                                           where prospect.ProsType == 1 &&
                                                 prosProd.Status == 0 &&
                                                 prosProd.FStage == "trail"
                                           select new { Prospect = prospect, ProsProd = prosProd };

            ViewBag.ClienttrailLicenceShared = clienttrailLicenceShared.Count();
            var totalInvoices = _context.PropInvoice.Count();
            ViewBag.TotalInvoices = totalInvoices;

            var invoicesAmount = _context.PropInvoice.Sum(p => p.InvoiceAmt);
            ViewBag.InvoicesAmount = invoicesAmount;

            var totalPaymentPending = _context.PropInvoice.Where(p => p.Status == 0).Sum(p => p.InvoiceAmt);
            ViewBag.TotalPaymentPending = totalPaymentPending;

            var totalPaymentReceived = _context.PropInvoice.Where(p => p.Status != 0).Sum(p => p.InvoiceAmt);
            ViewBag.TotalPaymentReceived = totalPaymentReceived;

            return View();
        }

        public async Task<IActionResult> PartnerProductIndex(string searchString, string sortOrder, int page = 1, int pageSize = 7)
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
    }
}
