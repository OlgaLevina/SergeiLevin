using Microsoft.AspNetCore.Mvc;
using SergeiLevin0.Interfaces;
using SergeiLevin0.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SergeiLevin0.Components
{
    //[ViewComponent(Name ="Category")] - замена наследованию от ViewComponent
    public class CategoriesViewComponent: ViewComponent //позволяет создать визуальной компоненс с именем Category
    {
        private readonly IProductData ProductData;

        public CategoriesViewComponent(IProductData productData) { ProductData = productData; }

        //метод обнаруживается с помощью рефлексии внутри класса 
        public IViewComponentResult Invoke() => View(GetCategories()); //синхронная реализация
        //public async Task<IViewComponentResult> InvokeAsync() => View(); //асинхронная реализация - тип реалищации выбирается в зависимости от назначения компонента

        private IEnumerable<CategoryViewModel> GetCategories()
        {
            var categories = ProductData.GetCategories();
            var parent_categories = categories.Where(category => category.ParentId is null).ToArray();
            var parent_categories_views=parent_categories.Select(parent_category => new CategoryViewModel
            { Id = parent_category.Id, Name = parent_category.Name, Order = parent_category.Order }).ToList();
            foreach (var parent_category_view in parent_categories_views)
            {
                var childs = categories.Where(category => category.ParentId == parent_category_view.Id);
                foreach (var child_category in childs)
                    parent_category_view.ChildCategories.Add(new CategoryViewModel
                    { Id = child_category.Id, Name = child_category.Name, Order = child_category.Order, ParentCategory=parent_category_view });
                parent_category_view.ChildCategories.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parent_categories_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_categories_views;
        }
    }
}
