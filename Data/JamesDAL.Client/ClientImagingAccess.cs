using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using James.Shared.Imaging;
using James.Shared.Model;
using static James.Shared.Imaging.IImagingAccess;

namespace James.Data.Client
{
    public class ClientImagingAccess: ImagingAccessBase
    {
        public override Task<ImagingDocument[]> SearchDocumentsAsync(string id, ImagingDocumentCategory docCategory,
            string? documentType = null)
        {
            throw new NotImplementedException();
        }

        public override Task<ImagingDocument[]> SearchDocumentsAsync(ImagingSearchCriteria criteria,
            KeyValuePair<string, string>[]? searchOptions = null,
            KeyValuePair<string, string>[]? additionalParams = null)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> UploadDocument(string docType, string filename, Stream fileContentStream, string contentType, ImagingDocumentCategory category, string id, string batchName, DateTime scanDate, CancellationToken cancellationToken = default)
        {
            //UNDONE:Send file to ImagingRepository/UploadDocument 
            var url = $"ImagingRepository/UploadDocument/{docType}/{(int)category}?filename={filename};documentId={id}";
            
            throw new NotImplementedException();
        }
    }
}
