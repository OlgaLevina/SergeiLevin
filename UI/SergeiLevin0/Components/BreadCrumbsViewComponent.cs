using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
