using System.Text.RegularExpressions;

namespace SharedBusinessLogic
{
    public static class ImagingBusinessLogic
    {
        public static Regex ValidWindowsFilenamePattern =
            new(@"^(?:.*\\)?(?<filename>[a-zA-Z0-9](?:[a-zA-Z0-9 ._-]*[a-zA-Z0-9])?)\.(?<extension>[a-z]+)$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        /// <summary>
        /// Validates that a filename is valid to be uploaded to the P8 imaging system
        /// </summary>
        /// <param name="filename">The filename being inspected.  Any path information is ignored.</param>
        /// <returns></returns>
        public static bool IsValidImagingFileName(string filename)
        {
            var match = ValidWindowsFilenamePattern.Match(filename);
            // ReSharper disable once PossibleUnintendedLinearSearchInSet
            return match.Success && AllowedFileTypes.Contains(match.Groups["extension"].Value,
                StringComparer.InvariantCultureIgnoreCase);
        }
        private static HashSet<string>? _allowedFileTypes;

        /// <summary>
        /// Gets the file types allowed by BTS central imaging team.
        /// </summary>
        /// <value>
        /// The allowed file types.
        /// </value>
        public static HashSet<string> AllowedFileTypes =>
            //NOTE:  This list is from documentation provided by the central BTS Imaging team.  Source document is in source control in the "Documents" solution folder.
            _allowedFileTypes ?? (_allowedFileTypes = new HashSet<string>
            {
                "mp3",
                "mp4",
                "wma",
                "rar",
                "zip",
                "msg",
                "eml",
                "tiff",
                "tif",
                "jpg",
                "jpeg",
                "gif",
                "png",
                "bmp",
                "psd",
                "ai",
                "svg",
                "doc",
                "dot",
                "docx",
                "dotx",
                "docm",
                "dotm",
                "rtf",
                "txt",
                "xls",
                "xlsx",
                "xlsm",
                "csv",
                "one",
                "ppt",
                "pps",
                "pptx",
                "pptm",
                "ppsx",
                "ppsm",
                "mpp",
                "vsd",
                "vsdx",
                "vsdm",
                "pdf",
                "avi",
                "mov",
                "mp4",
                "wmv",
                "vcf",
                "htm",
                "html",
                "xhtml",
                "mhtml",
                "mht",
                "xml"
            });
    }
}
