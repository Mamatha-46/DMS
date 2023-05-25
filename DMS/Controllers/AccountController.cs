using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DMS.Models;
using UserInterface.Controllers;

namespace DMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly cyberContext _cyber;
        CryptoHash crptoHash = new CryptoHash();
        public AccountController(cyberContext cyber)
        {
            _cyber = cyber;
        }

        [AllowAnonymous]

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(User user,string returnurl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //}


    }
}
