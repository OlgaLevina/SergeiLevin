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

namespace SergeiLevin0.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory UrlHelperFactory;

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

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
                li.AddCssClass("active");
            else
            {
                PageUrlValues["page"] = PageNumber;
                a.Attributes["href"] = Url.Action(PageAction, PageUrlValues);
            }

            a.InnerHtml.AppendHtml(PageNumber.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}

