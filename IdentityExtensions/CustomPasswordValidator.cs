﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebApplication1.IdentityExtensions
{
    public class CustomPasswordValidator : IIdentityValidator<string>

    {

        public int RequiredLength { get; set; }

        public CustomPasswordValidator(int length)

        {

            RequiredLength = length;

        }

        public Task<IdentityResult> ValidateAsync(string item)

        {

            if (String.IsNullOrEmpty(item) || item.Length < RequiredLength)

            {

                return Task.FromResult(IdentityResult.Failed(String.Format("Password should be of length {0}", RequiredLength)));

            }

            string pattern = @"[0-9a-zA-Z0-9]{10,}$";

            if (!Regex.IsMatch(item, pattern))

            {

                return Task.FromResult(IdentityResult.Failed("Password should have one numeral and one special character"));

            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}