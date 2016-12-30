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
            var parser = new BBCodeParser(new[]
{
                new BBTag("b", "<b>", "</b>"),
                new BBTag("i", "<span style=\"font-style:italic;\">", "</span>"),
                new BBTag("u", "<span style=\"text-decoration:underline;\">", "</span>"),
                new BBTag("code", "<pre class=\"prettyprint\">", "</pre>"),
                new BBTag("img", "<img src=\"${content}\" />", "", false, true),
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


            });
            return parser;
        }
    }
}