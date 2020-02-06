﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SergeiLevin0.Domain.DTO.Products
{
    public class PagedProductDTO
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public int TotalCount { get; set; }
    }
}
