using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.ViewModels
{
    public class BrandCompleteViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }
        public int? CurrentBrandId { get; set; }
    }
}
