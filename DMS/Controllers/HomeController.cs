using DMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DMS.BAL;

namespace DMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly cyberContext _context;

        public HomeController(ILogger<HomeController> logger, cyberContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            PdfGeneration pdfGeneration = new PdfGeneration();
            pdfGeneration.WriteToPdf("","");
            return View();
        }

        public IActionResult Dashboard()
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
            //var quotationShared = _context.Prospect.Join(_context.ProsProd,
            //                       prospect => prospect.Id,
            //                       prosProd => prosProd.ProsId,
            //                       (prospect, prosProd) => new { Prospect = prospect, ProsProd = prosProd })
            //                       .Where(joinResult => joinResult.Prospect.ProsType == 0 &&
            //                       joinResult.ProsProd.Status == 0 &&
            //                       joinResult.ProsProd.FStage == "quoteG")
            //                       .Count();

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
