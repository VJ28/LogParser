using System;
using System.Text.RegularExpressions;

namespace LogParser
{
    public static class Utility
    {
        public static string ReplaceIdInURL(string apiURL)
        {
            if(apiURL == string.Empty || apiURL == null){
                throw new ArgumentException("Invalid url");
            }
            return Regex.Replace(apiURL, @"/([\d]+)", "/{id}");
        }
    }
} 