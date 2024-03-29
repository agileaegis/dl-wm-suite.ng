﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace dl.wm.suite.auth.api.Helpers.Security
{
    public class HashHelper
    {
        public static string Sha512(string input)
        {

            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}