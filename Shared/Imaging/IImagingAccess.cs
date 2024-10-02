using James.Shared.Model;

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

        public Task<ImagingDocument[]> SearchDocumentsAsync(string id, ImagingDocumentCategory docCategory,
            string? documentType = null);
        public Task<ImagingDocument[]> SearchDocumentsAsync(ImagingSearchCriteria criteria,
            KeyValuePair<string, string>[]? searchOptions = null,
            KeyValuePair<string, string>[]? additionalParams = null);

        public ImagingSearchCriteria GetSearchCriteria(string id, ImagingDocumentCategory docCategory,
                                                        bool useDocCategoryAsCriteria = true);
    }
    public abstract class ImagingAccessBase : IImagingAccess
    {
        public abstract Task<Guid?> UploadDocument(string docType, string filename, 
                                            Stream fileContentStream,
            string contentType,
            ImagingDocumentCategory category, string id, string batchName, DateTime scanDate, CancellationToken cancellationToken = default);

        public abstract Task<ImagingDocument[]> SearchDocumentsAsync(string id, ImagingDocumentCategory docCategory,
            string? documentType = null);

        public abstract Task<ImagingDocument[]> SearchDocumentsAsync(ImagingSearchCriteria criteria,
            KeyValuePair<string, string>[]? searchOptions = null,
            KeyValuePair<string, string>[]? additionalParams = null);

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

        protected static readonly KeyValuePair<string, string>[] DefaultSearchOptions = { };

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

        public ImagingSearchCriteria GetSearchCriteria(string id, ImagingDocumentCategory docCategory,
                                                        bool useDocCategoryAsCriteria = true)
        {
            id = id.Trim();
            var criteria = new ImagingSearchCriteria()
            {
                MaxResults = 2000,
                Fields = string.Join(",", DocumentPropertyFields)
            };
            if (useDocCategoryAsCriteria)
            {
                criteria.DocClass = docCategory.DocumentCategory();
            }

            switch (docCategory)
            {
                case ImagingDocumentCategory.Account: //1
                    criteria.WhereClause = $"{AccountId} = '{id}'";
                    break;
                //TODO: Switch to IDataAccress
                //case ImagingDocumentCategory.Bond: //2
                //    criteria.WhereClause = $"{PolicyNo} = '{id}'";
                //    var myBidbondType = UtilityImaging.getBidNumber(id); //check for bid number.
                //    if (myBidbondType.bidID > -1)
                //        criteria.WhereClause =
                //            $"({criteria.WhereClause} OR {(string.Equals(myBidbondType.bondType.Trim(), "CONTRACT", StringComparison.InvariantCultureIgnoreCase) ? ContBidId : CommBidId)} = '{myBidbondType.bidID}')";
                //    break;
                case ImagingDocumentCategory.Agency: //3
                    criteria.WhereClause = $"{AgencyNo} = '{id}'";
                    break;
                //case ImagingDocumentCategory.CommBid: //4
                //    criteria.WhereClause = $"{CommBidId} = '{id}'";
                //    bondNum = UtilityImaging.GetBondForBid(int.Parse(id), "Commercial");
                //    if (!string.IsNullOrWhiteSpace(bondNum))
                //        criteria.WhereClause = $"({criteria.whereClause} OR {PolicyNo} = '{bondNum}')";
                //    break;
                //case ImagingDocumentCategory.ContBid: //5
                //    criteria.WhereClause = $"{ContBidId} = '{id}'";
                //    bondNum = UtilityImaging.GetBondForBid(int.Parse(id), "Contract");
                //    if (!string.IsNullOrWhiteSpace(bondNum))
                //        criteria.WhereClause = $"({criteria.WhereClause} OR {PolicyNo} = '{bondNum}')";
                //    break;
                case ImagingDocumentCategory.Billing: //SearchBillingDocuments
                    criteria.WhereClause = $"{AccountId} = '{id}''";
                    break;
                default:
                    throw new ArgumentException("Invalid docCategory", "docCategory");
            }
            return criteria;
        }
    }
}
