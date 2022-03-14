using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//196
namespace Spice.TagHelpers
{
    [HtmlTargetElement("div", Attributes ="page-model")]
    public class PageLinkTagHelper:TagHelper    //class implements TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory) //dependency injection
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound] //means this attribute isnt one that you intend to set via a taghelper attribute in a Html
        public ViewContext ViewContext { get; set; }

        //set the properties for the taghelper
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; } //to represent which action it should redirect to
        public bool PageClassesEnabled { get; set; }
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) //what we'll be doing inside this custom taghelper
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div"); //creates a div and inside it add an anchor tag ("a") below
            for(int i = 1; i<= PageModel.totalPage; i++) //displays the pagination(1,2,3,..)
            {
                TagBuilder tag = new TagBuilder("a"); //every page count has an anchor tag
                string url = PageModel.urlParam.Replace(":", i.ToString()); // in all of then it retreives url from urlParam and replaces the : with current page or i
                tag.Attributes["href"] = url;  //For <a> the href attribute specifies the URL the link goes to

                if (PageClassesEnabled)  //asigns CSS Classes
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal); //applys the pageClassSelected class only to current page, otherwise applys pageClassNormal
                }
                tag.InnerHtml.Append(i.ToString()); // i will be the content of our pagination
                result.InnerHtml.AppendHtml(tag);   // appends the created anchor tag to our main div (result)
                                                   
            }
            output.Content.AppendHtml(result.InnerHtml);   //once it's appended to the main div, we want to output it

            //  base.Process(context, output);  outo generated which has been overwritten
        }
    }
}
