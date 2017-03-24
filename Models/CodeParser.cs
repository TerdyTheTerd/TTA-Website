using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeKicker.BBCode;

namespace WebApplication1.Models
{
    public class CodeParser
    {
        public static BBCodeParser getParser()
        {
            //Create a new parser to remove all formatting for use on post activity
            var parser = new BBCodeParser(new[]
{
                new BBTag("b", "<b>", "</b>"),
                new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                new BBTag("img", "<img src=\"${content}\" class=\"img-responsive imgg\"/>", "", false, true), //Use Bootstraps img-responsive class as a defualt for cases where the image would need to be resized for mobile viewing
                new BBTag("quote", "<blockquote>", "</blockquote>"),
                new BBTag("list", "<ul>", "</ul>"),
                new BBTag("numList", "<ol>", "</ol>"),
                new BBTag("*", "<li>", "</li>"),
                new BBTag("url", "<a href=\"${href}\">", "</a>", new BBAttribute("href", ""), new BBAttribute("href", "href")),
                new BBTag("font", "<span style=\"font-family: ${tagattr}\">", "</span>", true, true, new BBAttribute("tagattr", ""), new BBAttribute("tagattr", "tagattr")),
                new BBTag("color", "<span style=\"color: ${tagattr}\">", "</span>", true, true, new BBAttribute("tagattr", ""), new BBAttribute("tagattr", "tagattr")),
                new BBTag("size", "<span style=\"font-size: ${tagattr}px\">", "</span>", true, true, new BBAttribute("tagattr", ""), new BBAttribute("tagattr", "tagattr")),
                new BBTag("indent", "<span style=\"padding-left: ${tagattr}em\">", "</span>", true, true, new BBAttribute("tagattr", ""), new BBAttribute("tagattr", "tagattr")),
                new BBTag("sub", "<sub>", "</sub>"),
                new BBTag("sup", "<sup>", "</sup>"),
                new BBTag("center", "<p style=\"text-align: center\">", "</p>"),
                new BBTag("br", "<br>", "</br>")


            });
            return parser;
        }
    }
}