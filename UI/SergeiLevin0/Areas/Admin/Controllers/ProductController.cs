using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Domain.Entities.Identity;
using SergeiLevin0.Interfaces;

namespace SergeiLevin0.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class ProductController : Controller
    {

        private readonly IProductData ProductData;

        public ProductController(IProductData productData) => ProductData = productData;

        public IActionResult List() => View(ProductData.GetProducts());
        public IActionResult Edit(int? id) => View();//потом доработать представления по редактированию!!!
        public IActionResult Delete(int? id) => View();
        [HttpPost, ActionName(nameof(Delete))]
        public IActionResult DeleteConfirm(int? id) => RedirectToAction("List");
    }
}