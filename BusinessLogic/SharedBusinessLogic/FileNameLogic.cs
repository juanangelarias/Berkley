using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using James.Shared.Model;

namespace SharedBusinessLogic
{
    public static class FileNameLogic
    {
        private static readonly Regex _hasLlcPattern = new(@"[\s\W]*?LLC\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _hasIncPattern = new(@"[\s\W]*?Inc\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _illegalFilenameCharacters = new(@"[:/\\\<\>""\|\?\*]");

        //TODO:  Make Unit tests after Matt gives final version
        private static string ToFileName(string rawFileName)
        {
            //TODO:Get final version as a business rule from Matt
            var fileName = rawFileName;
            //Remove illegal characters
            fileName = _illegalFilenameCharacters.Replace(fileName,  "");

            //If has LLC in name, truncate just before the LLC
            var llcMatch = _hasLlcPattern.Match(fileName);
            if (llcMatch.Success)
                fileName = fileName.Substring(0, llcMatch.Index - 1).Trim();
            //If has Inc in name, truncate just before the Inc
            var incMatch = _hasIncPattern.Match(fileName);
            if (incMatch.Success)
                fileName = fileName.Substring(0, incMatch.Index - 1).Trim();
            return fileName.Substring(0,Math.Min(75,fileName.Length)).TrimEnd();
        }
        public static string ToFileName(this Account account) => ToFileName(account.IdNavigation.FullName);
        public static string ToFileName(this Agency agency) => ToFileName(agency.IdNavigation.FullName);
        public static string ToFileName(this Obligee obligee) => ToFileName(obligee.IdNavigation.FullName);
    }
}
