using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.HtmlHelpers
{
    public static class PagingLinks
    {
        public static MvcHtmlString GeneratePageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            StringBuilder linksHtml = new StringBuilder();
            TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml = "Page " + currentPage + " of " + totalPages;
            TagBuilder paginationContainer = new TagBuilder("div");
            paginationContainer.AddCssClass("pagination-container");
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");

            //Generate Arrow Links to PrevPage
            if (currentPage != 1)
            {
                TagBuilder prevPage = new TagBuilder("li");
                TagBuilder prevUrl = new TagBuilder("a");
                prevUrl.InnerHtml = "<";
                prevUrl.MergeAttribute("href", pageUrl(currentPage - 1));
                prevPage.IdAttributeDotReplacement += prevUrl.ToString();
                ulTag.InnerHtml += prevPage.ToString();
            }
            
            //Generate <li> elements containing hyperlinks
            for (int i = 1; i <= totalPages; i++)
            {
                TagBuilder liTag = new TagBuilder("li");
                TagBuilder aTag = new TagBuilder("a");
                aTag.InnerHtml = i.ToString();
                if (i == currentPage)
                {
                    liTag.AddCssClass("active");
                }
                else
                {
                    aTag.MergeAttribute("href", pageUrl(i));
                }
                liTag.InnerHtml += aTag.ToString();
                ulTag.InnerHtml += liTag.ToString();

                //OUTPUT
                //<ul>
                //  <li>
                //      <a href = link to page i>
                //  </li>
                //  <li>
                //      <a href = link to page i+1>
                //  </li>
                // </ul>
            }
            //Add links to nextPage
            if (currentPage != totalPages)
            {
                TagBuilder nextPage = new TagBuilder("li");
                TagBuilder nextUrl = new TagBuilder("a");
                nextUrl.InnerHtml = ">";
                nextUrl.MergeAttribute("href", pageUrl(currentPage + 1));
                nextPage.InnerHtml += nextUrl.ToString();
                ulTag.InnerHtml += nextPage.ToString();
            }
            //
            paginationContainer.InnerHtml += ulTag.ToString();
            divTag.InnerHtml += paginationContainer.ToString();
            linksHtml.Append(divTag.ToString());
            return MvcHtmlString.Create(linksHtml.ToString());

            //OUTPUT CODE:
            //<div>
            //     Page 1 of 3
            //        <div class="pagination-container"> 
            //            <ul class="pagination"> 
            //            </ul> 
            //        </div> 
            //</div> 

           
        }
    }
}