using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SergeiLevin0.Domain.Entities;

namespace SergeiLevin0.Components
{
    //[ViewComponent(Name ="Category")] - замена наследованию от ViewComponent
    public class CategoriesViewComponent: ViewComponent //позволяет создать визуальной компоненс с именем Category
    {
        private readonly IProductData ProductData;

        public CategoriesViewComponent(IProductData productData) { ProductData = productData; }

        //метод обнаруживается с помощью рефлексии внутри класса 
        //public IViewComponentResult Invoke() => View(GetCategories()); //синхронная реализация
        public IViewComponentResult Invoke(string CategoryId)
        {
            var category_id = int.TryParse(CategoryId, out var id) ? id : (int?)null;

            var categories = GetCategories(category_id, out var parent_category_id);

            return View(new CategoryCompleteViewModel
            {
                Categories = categories,
                CurrentCategoryId = category_id,
                CurrentParentCategory = parent_category_id
            });
        }        //public async Task<IViewComponentResult> InvokeAsync() => View(); //асинхронная реализация - тип реалищации выбирается в зависимости от назначения компонента

        private IEnumerable<CategoryViewModel> GetCategories(int? CategoryId, out int? ParentCategoryId)
        {
            var categories = ProductData.GetCategories();
            ParentCategoryId = null;
            //var parent_categories = categories.Where(category => category.ParentId is null).ToArray();
            //var parent_categories_views=parent_categories.Select(parent_category => new CategoryViewModel
            var parent_categories=categories.Where(category => category.ParentId is null)
                .ToArray().Select(parent_category=> new CategoryViewModel
                { Id = parent_category.Id, Name = parent_category.Name, Order = parent_category.Order }).ToList();
            foreach (var parent_category in parent_categories)
            {
                var childs = categories.Where(category => category.ParentId == parent_category.Id);
                foreach (var child_category in childs)
                {
                    if (child_category.Id == CategoryId)
                        ParentCategoryId = parent_category.Id;
                    parent_category.ChildCategories.Add(new CategoryViewModel
                    { Id = child_category.Id, Name = child_category.Name, Order = child_category.Order, ParentCategory = parent_category });
                    parent_category.ChildCategories.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
                }
            }
            parent_categories.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_categories;
        }
    }
}
