using FileNetP8SoapService;
using James.Shared;
using James.Shared.Imaging;
using James.Shared.Model;
using System.Drawing;
using System.Runtime.Versioning;
using System.ServiceModel;
using MimeTypes = James.Shared.Imaging.MimeTypes;

namespace James.Data.Imaging
{
    /// <summary>
    /// Imaging access from server-side code
    /// </summary>
    /// <remarks>Contains IImagingAccess plus server-only code for upload and download</remarks>
    [SupportedOSPlatform("windows")]
    public partial class ServerImagingAccess(ILoggingService loggingService, ImagingKong0Helper kong0Helper) : ImagingAccessBase
    {
        //public const string DocNumber = "F_DOCNUMBER";
        //public const string DocType = "DocType";
        //public const string ScanDate = "ScanDate";
        //public const string DocRemarks = "DocRemarks";
        //public const string ContBidId = "ContbidID";
        //public const string CommBidId = "CommbidID";
        //public const string AccountId = "AccountID";
        //public const string AgencyNo = "AgencyNo";
        //public const string PolicyNo = "PolicyNo";
        //public const string CompanyNo = "CompanyNo";
        //public const string BatchName = "BatchName";
        //public const string EntryDate = "F_ENTRYDATE";
        private static readonly P8ServiceClient _p8Client = new();

        public async Task<(Stream, string, string)> GetFileStreamAsync(Guid documentGuid, string docType)
        {
            async Task<document> P8Call(ClientBase<P8Service> client) => await ((P8Service) client).getDocumentByIDAsync(string.Empty, string.Empty, docType, documentGuid.ToString(), false, AdditionalParams(_includeFilenameParam));
            var doc =  await kong0Helper.ExecuteMethodAsync(_p8Client, P8Call);
            //var doc = await await kong0Helper.ExecuteMethodAsync(_p8Client, async client => await ((P8ServiceClient)client).getDocumentByIDAsync(string.Empty, string.Empty, docType, documentGuid.ToString(), false,
            //    AdditionalParams(_includeFilenameParam)));
            return await GetFileStreamAsync(doc!);

        }

        public async Task<(Stream, string, string)> GetFileStreamAsync(document doc)
        {
            var filename = doc.contentList[0].fileName ?? doc.GetProperty("Filename") ?? doc.GetProperty(DocRemarks) ?? Guid.NewGuid().ToString();
            if (null == doc.contentList || doc.contentList.Length == 0) return (new MemoryStream(0), string.Empty, string.Empty);

            if (doc.contentList.Length > 1 && MimeTypes.AreInterchangeableMimeTypes(doc.contentList[0].mimeType, "image/tiff"))
                try
                {
                    var pages = doc.contentList.Select(c => (c.contentMTOM ?? c.content1)).ToArray();
                    //Send streams instead of bytes to TiffHelper for less GC
                    var tiffImages = new List<Image>();
                    //Some TIFF files aren't actually TIFF files.  They need to be converted prior to being concatenated.
                    foreach (var page in pages)
                    {
                        using var originalStream = new MemoryStream(page);
                        var tiffImage = Image.FromStream(originalStream);
                        tiffImages.Add(tiffImage);
                    }

                    //TIFF files are split into individual pages by P8, and need to be reassembled into a single multi page TIFF
                    return (TiffHelper.MergeTiffToStream(tiffImages), filename, "image/tiff");
                }
                catch (Exception ex)
                {
                    loggingService.LogException(ex, "Combining TIFF files failed.", "Sending as simple concatenation.", severity: Severity.Warning);
                }
            var mimeType = doc.contentList[0].mimeType;
            if (string.IsNullOrWhiteSpace(mimeType))
                mimeType = MimeTypes.GetContentType(doc.contentList[0].fileName);
            return (new MemoryStream(doc.contentList.SelectMany(c => (c.contentMTOM ?? c.content1).ToList()).ToArray()), filename, mimeType);

        }

        private static readonly entry _includeFilenameParam = new(key: "includeFilenameProperties",
            value: "true");
        private static entry[] AdditionalParams(entry param)
        {
            var parms = DefaultAdditionalParams.Select(ap => ap.ToEntry()).ToList();
            parms.Add(param);
            return parms.ToArray();
        }

        public override async Task<Guid?> UploadDocument(string docType, string filename, Stream fileContentStream, string contentType,
            ImagingDocumentCategory category, string id, string batchName, DateTime scanDate, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!IsAllowedFileType(contentType))
                    throw new ArgumentException(
                        "This file type (" + contentType + ") is not supported by the P8 imaging system.  Contact BTS to add this file type if it is needed.");
                if ((int)category > 5)
                    throw new ArgumentException(
                        $"Document category {category.DocumentCategory()} is not supported by this method.",
                        "category");
                var contentMimeType = MimeTypes.MimeTypeFromExtension(contentType) ?? contentType;
                var remarks = filename;
                if (!filename.EndsWith("." + contentType))
                {
                    contentType = MimeTypes.ExtensionFromMimeType(contentType) ?? contentType;
                    filename = Path.ChangeExtension(filename, contentType);
                }
                var propertyList = new List<property>
                {
                    new() {name = DocType, value = docType},
                    new() {name = DocRemarks, value = remarks},
                    new() {name = CompanyNo, value = "27"}, //27 is the code for BSG
                    new() {name = BatchName, value = batchName}
                };
                switch (category)
                {
                    case ImagingDocumentCategory.Account:
                        propertyList.Add(new property { name = AccountId, value = id });
                        propertyList.Add(new property { name = ScanDate, dateValue = scanDate });
                        propertyList.Add(new property { name = EntryDate, dateValue = scanDate });
                        break;
                    case ImagingDocumentCategory.Bond:
                        propertyList.Add(new property { name = PolicyNo, value = id });
                        break;
                    case ImagingDocumentCategory.Agency:
                        propertyList.Add(new property { name = AgencyNo, value = id });
                        propertyList.Add(new property { name = ScanDate, dateValue = scanDate });
                        break;
                    case ImagingDocumentCategory.CommBid:
                        propertyList.Add(new property { name = CommBidId, value = id });
                        propertyList.Add(new property { name = ScanDate, dateValue = scanDate });
                        break;
                    case ImagingDocumentCategory.ContBid:
                        propertyList.Add(new property { name = ContBidId, value = id });
                        propertyList.Add(new property { name = ScanDate, dateValue = scanDate });
                        break;
                }

                byte[] buffer = new byte[fileContentStream.Length];
                // ReSharper disable once MustUseReturnValue
                fileContentStream.Read(buffer, 0, (int)fileContentStream.Length);
                var doc = new document
                {
                    documentClass = category.DocumentCategory(),
                    properties = propertyList.ToArray(),
                    contentList =
                    [
                        new content
                        {
                            contentMTOM = buffer,
                            fileName = filename,
                            mimeType = contentMimeType
                        }
                    ]
                };
                if (cancellationToken.IsCancellationRequested == false)
                {
                    async Task<string> P8Call(ClientBase<P8Service> client) =>
                        await ((P8Service)client).addDocumentAsync(
                            string.Empty, string.Empty,
                            doc, batchName, DefaultAdditionalParams.Select(ap => ap.ToEntry()).ToArray());

                    doc.guid = await kong0Helper.ExecuteMethodAsync(_p8Client, P8Call);
                    return Guid.Parse(doc.guid);
                }
                return null;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Error uploading to BTS controlled P8 servers.", category: "Imaging");
                return null;
            }
        }

        /// <summary>
        /// Searches the documents.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <param name="searchOptions">The search options.</param>
        /// <param name="additionalParams">The additional parameters.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Criteria must contain a document category;<paramref name="criteria"/></exception>
        public override async Task<ImagingDocument[]> SearchDocumentsAsync(ImagingSearchCriteria criteria,
            KeyValuePair<string, string>[]? searchOptions = null,
            KeyValuePair<string, string>[]? additionalParams = null)
        {
            if (string.IsNullOrWhiteSpace(criteria.DocClass))
                throw new ArgumentException("Criteria must contain a document category", "criteria");
            var p8Criteria = ThisToThat.ToEntityType<searchCriteria>(criteria);
            async Task<document[]> P8Call(ClientBase<P8Service> client) => (await ((P8Service)client).searchCurrentDocumentsAsync(new searchCurrentDocuments( string.Empty, string.Empty, [p8Criteria], true,
                (searchOptions ?? DefaultSearchOptions).Select(so => so.ToEntry()).ToArray(),
                (additionalParams ?? DefaultAdditionalParams).Select(so => so.ToEntry()).ToArray()))).documentList;
            var foundDocuments = await kong0Helper.ExecuteMethodAsync(_p8Client, P8Call);
            return foundDocuments?.Select(fd => fd.ToImagingDocument()).ToArray()??[];
        }

        /// <summary>
        /// Searches for all documents connected to an id and an (optional) doc type.
        /// </summary>
        /// <param name="id">The connected id.</param>
        /// <param name="docCategory">The <see cref="document"/> category.</param>
        /// <param name="documentType">Document type (optional).</param>
        /// <returns>All found documents</returns>
        public override async Task<ImagingDocument[]> SearchDocumentsAsync(string id, ImagingDocumentCategory docCategory,
            string? documentType = null)
        {
            var searchCriteria = GetSearchCriteria(id, docCategory);
            if (null != documentType)
                searchCriteria.WhereClause += " AND " + DocType + " = '" + documentType + "'";

            var results = await SearchDocumentsAsync(searchCriteria);
            return results;
        }

        private static Dictionary<ImagingDocumentCategory, ImagingProperty[]> _availableProperties =
            new(6);

        public async Task<ImagingProperty[]> GetAvailableImagingPropertiesAsync(ImagingDocumentCategory category)
        {
            if (_availableProperties.TryGetValue(category, out ImagingProperty[]? value))
                return value;

            async Task<getPropertyMappingsResponse> P8Call(ClientBase<P8Service> client) =>
                await ((P8Service)client).getPropertyMappingsAsync(new getPropertyMappings( string.Empty, string.Empty,
                    category.DocumentCategory(),
                    DefaultAdditionalParams.Select(so => so.ToEntry()).ToArray()));

            var propertyMappings = await kong0Helper.ExecuteMethodAsync(_p8Client, P8Call);
            return _availableProperties[category] = propertyMappings?.properties.Select(p => p.ToImagingProperty())
                .ToArray()??[];
        }

        private static Dictionary<ImagingDocumentCategory, ImagingProperty[]>? _validProperties;

        public Dictionary<ImagingDocumentCategory, ImagingProperty[]> ValidProperties
        {
            get
            {
                return _validProperties ??= Enumerable.Range(1, 6)
                    .Cast<ImagingDocumentCategory>()
                    .Select(
                        cat =>
                            new Tuple<ImagingDocumentCategory, ImagingProperty[]>(cat,
                                GetAvailableImagingPropertiesAsync(cat).Result))
                    .ToDictionary(t => t.Item1, t => t.Item2);
            }
        }
    }
}
