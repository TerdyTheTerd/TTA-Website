using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null || file.ContentLength > 4 * 1024 * 1024)
            {
                return false;
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg))
                    {
                        return true;
                    }
                }
            } catch { }
            return false;

        }
    }
}