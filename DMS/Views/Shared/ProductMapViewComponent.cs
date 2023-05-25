using DMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Views.Shared
{
    public class ProductMapViewComponent:ViewComponent
    {
        private readonly IPostRepository _ipr;
        public ProductMapViewComponent(IPostRepository ipr)
        {
            _ipr = ipr;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var distriDocu = await _ipr.GetProductMapById(Id);
            return View("_GetProductMapById", distriDocu);
        }
    }
}
