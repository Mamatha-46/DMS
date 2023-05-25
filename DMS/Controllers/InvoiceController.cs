using DMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IPostRepository _ipr;
        public InvoiceController(IPostRepository ipr)
        {
            _ipr = ipr;
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pagesize = 5)
        {
            var invoices = await _ipr.GetAllInvoices();
            if (!string.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(b =>
    (b.InvoiceNo != null && b.InvoiceNo.Contains(searchString)) ||
    (b.From != null && b.From.Contains(searchString)) ||
    (b.To != null && b.To.Contains(searchString)) ||
    (b.Amount != null && b.Amount.Contains(searchString)) ||
    (b.AddedOn != null && b.AddedOn.ToString().Contains(searchString))
    );

            }
            switch (sortOrder)
            {
                case "name":
                    invoices = invoices.OrderBy(b => b.InvoiceNo);
                    break;
                case "name_desc":
                    invoices = invoices.OrderByDescending(b => b.InvoiceNo);
                    break;
                case "type":
                    invoices = invoices.OrderBy(b => b.From);
                    break;
                case "type_desc":
                    invoices = invoices.OrderByDescending(b => b.From);
                    break;
                case "country":
                    invoices = invoices.OrderBy(b => b.To);
                    break;
                case "country_desc":
                    invoices = invoices.OrderByDescending(b => b.To);
                    break;
                case "state":
                    invoices = invoices.OrderBy(b => b.Amount);
                    break;
                case "state_desc":
                    invoices = invoices.OrderByDescending(b => b.Amount);
                    break;
                case "city":
                    invoices = invoices.OrderBy(b => b.AddedOn);
                    break;
                case "city_desc":
                    invoices = invoices.OrderByDescending(b => b.AddedOn);
                    break;
            }
            //  brandinfo = brandinfo.OrderBy(b => b.BrandName);

            return View(invoices);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _ipr.GetInvoicesById(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
    }
}
