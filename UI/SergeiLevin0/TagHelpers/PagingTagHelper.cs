using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SergeiLevin0.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SergeiLevin0.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory UrlHelperFactory;

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]//если не добавить, то при переходе со страницы на стануцу перестают правильно работать хлебные крошки (исчетает бренд)
        //это происходит, т.к. атрибуты page-url-... на странице разметке данный тэг не рассматривает в качестве атрибутов, поэтому через атрибует связываем наш словарь с соответствующими префиксами
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory) => UrlHelperFactory = urlHelperFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url_helper = UrlHelperFactory.GetUrlHelper(ViewContext);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            for (int i = 1, total_pages = PageModel.TotalPages; i < total_pages; i++)
                ul.InnerHtml.AppendHtml(CreateItem(i, url_helper));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateItem(int PageNumber, IUrlHelper Url)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if (PageNumber == PageModel.PageNumber)
            {
                a.MergeAttribute("data-page", PageModel.PageNumber.ToString());//добавляем к кнопке текущей страницы соответствующий атрибут с ее номером
                li.AddCssClass("active");//делаем кнопку текущей страницы активной
            }
            else
            {
                PageUrlValues["page"] = PageNumber;
                a.Attributes["href"] = "#";//Url.Action(PageAction, PageUrlValues);//подавляем стандартное поведение ссылки
                foreach (var (key, value) in PageUrlValues.Where(p => p.Value != null))//добавляем все элементы из словаря обратно в ссылку, т.к. раньше они были в параметрах маршрута, а мы их теперь аннулировали вмете со ссылкой юрл из href. А они нам нужны в будущем для построения запросов к серверу
                    a.MergeAttribute($"data-{key}", value.ToString());
            }

            a.InnerHtml.AppendHtml(PageNumber.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}

