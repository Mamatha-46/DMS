using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Repository;

namespace DMS.Views.Shared
{
    public class DocumentDetailsViewComponent:ViewComponent
    {
        private readonly IPostRepository _ipr;
        public DocumentDetailsViewComponent(IPostRepository ipr)
        {
            _ipr = ipr;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var distriDocu = await _ipr.GetDistriDocuByID(Id);
            return View("_UpdateDistriDocuPar",distriDocu);
        }
    }
}
