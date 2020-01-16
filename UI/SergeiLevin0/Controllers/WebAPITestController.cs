using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces.Api;

namespace SergeiLevin0.Controllers
{
    public class WebAPITestController : Controller
    {
        private readonly IValuesService valuesService;

        public WebAPITestController(IValuesService valuesService)=>this.valuesService = valuesService;
        public async Task<IActionResult> Index()
        {
            var values = await valuesService.GetAsync();
            return View(values);
        }
    }
}