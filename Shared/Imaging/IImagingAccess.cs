namespace James.Shared.Imaging
{
    /// <summary>
    /// Contract for interfacing with the imaging system
    /// </summary>
    public interface IImagingAccess
    {
        public Task<Guid?> UploadDocument(string docType, string filename, Stream fileContentStream,
            string contentType, ImagingDocumentCategory category, string id, 
            string batchName, DateTime scanDate, CancellationToken cancellationToken = default);
    }

    //TODO:Merge this with ServerImagingAccess
    public abstract class ImagingAccessBase : IImagingAccess
    {
        public abstract Task<Guid?> UploadDocument(string docType, string filename, 
                                            Stream fileContentStream,
            string contentType,
            ImagingDocumentCategory category, string id, string batchName, DateTime scanDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the file types allowed by BTS central imaging team.
        /// </summary>
        protected readonly string[] AllowedFileTypes =
        [
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
        ];

        public const string DocNumber = "F_DOCNUMBER";
        public const string DocType = "DocType";
        public const string ScanDate = "ScanDate";
        public const string DocRemarks = "DocRemarks";
        public const string ContBidId = "ContbidID";
        public const string CommBidId = "CommbidID";
        public const string AccountId = "AccountID";
        public const string AgencyNo = "AgencyNo";
        public const string PolicyNo = "PolicyNo";
        public const string CompanyNo = "CompanyNo";
        public const string BatchName = "BatchName";
        public const string EntryDate = "F_ENTRYDATE";
        public const string Filename = "Filename";
        public const string MimeType = "MimeType";
        /// <summary>
        /// Typical fields returned for most document search results.
        /// </summary>
        public static readonly string[] DocumentPropertyFields = new[]
        {
            DocNumber,
            DocType,
            ScanDate,
            DocRemarks
        };

        protected static readonly KeyValuePair<string, string>[] DefaultSearchOptions = [];

        protected static readonly KeyValuePair<string, string>[] DefaultAdditionalParams =
        [
            new( "applicationName",  "JamesTheBondSystem")
        ];
        /// <summary>
        /// Determines whether the specified extension or Mime type is allowed by
        /// BTS imaging team to be uploaded into P8.
        /// </summary>
        /// <param name="extensionOrMimeType">The extension or mime type.</param>
        /// <returns>True if allowed to be uploaded.</returns>
        public bool IsAllowedFileType(string extensionOrMimeType)
        {
            if (AllowedFileTypes.Contains(extensionOrMimeType.TrimStart(".".ToCharArray()).ToLowerInvariant())) return true;
            var ext = MimeTypes.ExtensionFromMimeType(extensionOrMimeType);
            return (null != ext && AllowedFileTypes.Contains(ext));
        }
    }
}
