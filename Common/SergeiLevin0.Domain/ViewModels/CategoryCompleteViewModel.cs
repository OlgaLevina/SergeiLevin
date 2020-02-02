using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.ViewModels
{
    public class CategoryCompleteViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public int? CurrentParentCategory { get; set; }

        public int? CurrentCategoryId { get; set; }
    }
}
