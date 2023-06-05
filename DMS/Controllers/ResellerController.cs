using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Repository;
using DMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMS.Controllers
{
    public class ResellerController : Controller
    {

        private readonly IPostRepository _ipr;
        cyberContext _context;
        
        public ResellerController(IPostRepository ipr, cyberContext context)
        {
            _context = context;
            _ipr = ipr;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
           
    }
}
