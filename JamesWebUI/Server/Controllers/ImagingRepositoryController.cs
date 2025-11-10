using James.Data.Imaging;
using James.Shared;
using James.Shared.Data;
using James.Shared.Imaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;
using System.Text.Json;
using James.Shared.Constants;

namespace JamesWebUI.Server.Controllers
{
    /// <summary>
    /// This controller is for uploading and downloading imaging documents
    /// </summary>
    /// <remarks>All other imaging functions should be in an IImagingAccess object</remarks>
    /// <param name="imagingAccess">Exposes imaging functions to the application</param>
    /// <param name="loggingService">Exposes logging functions to the application</param>
    [SupportedOSPlatform("windows")]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImagingRepositoryController(ServerImagingAccess imagingAccess, ILoggingService loggingService)
    {
        private ServerImagingAccess ServerImagingAccess => imagingAccess;

        /// <summary>
        /// Gets the document from imaging.
        /// </summary>
        /// <param name="documentGuid">The document unique identifier.</param>
        /// <param name="documentCategory">Document class.</param>
        /// <returns>The file from imaging</returns>
        [HttpGet("GetDocument/{documentCategory}/{documentGuid:guid}")]
        public async Task<ActionResult> GetDocument(Guid documentGuid, ImagingDocumentCategory documentCategory)
        {
            try
            {
                var (filestream, filename, contentType) = await ServerImagingAccess.GetFileStreamAsync(documentGuid, documentCategory);
                //TODO: Check if imaging system has a file date that can be passed in below as LastModifiedDate
                return new FileStreamResult(filestream, contentType) { FileDownloadName = filename };
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception getting document from imaging.", category: "Imaging");
                //TODO:Remove this once unhandled exceptions are handled and logged 
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Upload a single document
        /// </summary>
        /// <param name="file">The IFormFile to upload</param>
        /// <param name="docType">doctype to upload as</param>
        /// <param name="category">ImagingDocumentCategory cast as an int</param>
        /// <param name="id">Imaging Id (Account number for accounts, etc.)</param>
        /// <param name="batch">Any meaningful string to identify batch</param>
        /// <param name="description">Document description</param>
        /// <param name="cancellationToken"></param>
        /// <returns>StatusCodeResult</returns>
        [HttpPost("UploadDocument/{docType}/{category:int}/{id}")] //?documentId={documentId};description={description}
        public async Task<ActionResult> UploadDocument(IFormFile file, string docType, int category, string id, [FromQuery] string batch = "", [FromQuery] string description = "", CancellationToken cancellationToken = default)
        {
            var fileType = file.FileName.Split('.').Last();
            if (batch.Length == 0) batch = description;
            if (!ServerImagingAccess.IsAllowedFileType(fileType))
            {
                loggingService.LogError("Unsupported File type",
                    "FileType is not supported by the P8 imaging system.  Contact BTS to add this file type if it is needed.",
                    "Imaging", new Dictionary<string, string>() { { "FileType", fileType } });
                return new StatusCodeResult(422);//Unprocessable content
            }
            if (string.IsNullOrWhiteSpace(batch)) batch = file.FileName;
            if (cancellationToken.IsCancellationRequested)
            {
                return new StatusCodeResult(409);//Conflict (between making and cancelling the request)
            }
            
            var guid = await ServerImagingAccess.UploadDocument(docType, string.IsNullOrWhiteSpace(description) ? file.Name : description,
                file.OpenReadStream(),
                file.ContentType, (ImagingDocumentCategory)category,
                id, batch, DateTime.Now);
            var jsonResult = JsonSerializer.Serialize(new { DocumentID = guid });
            //TODO:Handle errors from imaging system
            return new ContentResult { Content = JsonSerializer.Serialize(jsonResult), ContentType = "application/json", StatusCode = 200 };
        }

        /// <summary>
        /// Upload multiple documents to imaging
        /// </summary>
        /// <param name="files">Files to upload</param>
        /// <param name="docTypes">Document types to upload as, separated by a ':'</param>
        /// <param name="category">ImagingDocumentCategory cast as an int</param>
        /// <param name="id">Imaging Id (Account number for accounts, etc.)</param>
        /// <param name="batch">Any meaningful string to identify batch</param>
        /// <param name="descriptions">Descriptions of the files, for filenet</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>StatusCodeResult</returns>
        [HttpPost("UploadDocuments/{docTypes}/{category:int}/{id}")]
        public async Task<StatusCodeResult> UploadDocuments(IFormFileCollection files, string docTypes, int category, string id, [FromQuery] string batch = "", [FromQuery] string descriptions = "", CancellationToken cancellationToken = default)
        {
            //Pre-validate
            var unsupportedFiles = files
                .Where(uf => ServerImagingAccess.IsAllowedFileType(uf.FileName.Split('.').Last()) == false).ToArray();
            if (unsupportedFiles.Length != 0)
            {
                var errorData =
                    Enumerable.Range(1, unsupportedFiles.Length)
                    .ToDictionary(i =>
                        unsupportedFiles[i].Name, i => unsupportedFiles[i].ContentType);
                loggingService.LogError("Unsupported File type(s)",
                    "A document with an unsupported file type detected.  Contact BTS to add this file type if it is needed.",
                    "Imaging", errorData);
                return new StatusCodeResult(422);//Unprocessable content
            }
            try
            {
                if (string.IsNullOrWhiteSpace(batch))
                    //NOTE:  I'm not sure whether the batch is used for anything.  Just taking a stab at something maybe useful.
                    batch = $"James{category}-{files.Count}-{DateTime.Now.ToShortDateString()}";
                var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
                var allTypes = docTypes.Split(':');
                var allDescriptions = descriptions.Split(':');
                if (files.Count != allTypes.Length || files.Count != allDescriptions.Length)
                {
                    loggingService.LogError("Invalid descriptions and/or document types error",
                        "The amount of descriptions, document types and upload files was not the same",
                        StandardLoggingCategories.Imaging,
                        new Dictionary<string, string> { { "Types submitted", allTypes.Length.ToString() }, { "Filenames submitted", allDescriptions.Length.ToString() }, { "Uploaded files", files.Count.ToString() } });
                    return new StatusCodeResult(422);//Unprocessable content
                }
                var docsWithInfo = Enumerable.Range(0, files.Count).Select(i => new DocumentTypeFileName
                { Description = allDescriptions[i], DocumentType = allTypes[i], UploadedFile = files[i] });

                if (cancellationToken.IsCancellationRequested)
                {
                    return new StatusCodeResult(409);//Conflict (between making and cancelling the request)
                }
                await Parallel.ForEachAsync(docsWithInfo, options, async (uf, ct) => await UploadDocument(uf.UploadedFile, uf.DocumentType, category, id, batch, uf.Description, ct));

                if (cancellationToken.IsCancellationRequested)
                {
                    return new StatusCodeResult(409);//Conflict (between making and cancelling the request)
                }
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Error uploading files to imaging", category: "Imaging");
                return new StatusCodeResult(500);
            }
        }

        private class DocumentTypeFileName
        {
            public required string DocumentType { get; set; }
            public required IFormFile UploadedFile { get; set; }
            public required string Description { get; set; }
        }
    }
}
