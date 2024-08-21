using FileNetP8SoapService;
using James.Data.Imaging;
using James.Shared;
using James.Shared.Imaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;

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
    public class ImagingRepositoryController(IImagingAccess imagingAccess, ILoggingService loggingService)
    {
        private ServerImagingAccess ServerImagingAccess => (ServerImagingAccess)imagingAccess;

        /// <summary>
        /// Gets the <see cref="document"/>.
        /// </summary>
        /// <param name="documentGuid">The document unique identifier.</param>
        /// <param name="docType">Type of the document.</param>
        /// <returns></returns>
        [HttpGet("GetDocument/{docType}/{documentGuid:guid}")]
        public async Task<ActionResult> GetDocument(Guid documentGuid, string docType)
        {
            try
            {
                var (filestream, filename, contentType) = await ServerImagingAccess.GetFileStreamAsync(documentGuid, docType);
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
        /// <param name="uploadedFile">The IFormFile to upload</param>
        /// <param name="docType">doctype to upload as</param>
        /// <param name="category">ImagingDocumentCategory cast as an int</param>
        /// <param name="batch">Any meaningful string to identify batch</param>
        /// <returns></returns>
        [HttpPost("UploadDocument/{docType}/{category:int}")]
        public async Task<StatusCodeResult> UploadDocument(IFormFile uploadedFile, string docType, int category, string batch = "")
        {
            var fileType = uploadedFile.FileName.Split('.').Last();
            if (!ServerImagingAccess.IsAllowedFileType(fileType))
            {
                loggingService.LogError("Unsupported File type", 
                    "FileType is not supported by the P8 imaging system.  Contact BTS to add this file type if it is needed.", 
                    "Imaging", new Dictionary<string, string>() { { "FileType", fileType } });
                return new StatusCodeResult(422);//Unprocessable content
            }
            var docId = new Guid().ToString();
            if (string.IsNullOrWhiteSpace(batch)) batch = uploadedFile.FileName;
            //TODO:Figure out what Batch is supposed to be
            await ServerImagingAccess.UploadDocument(docType, uploadedFile.Name, 
                uploadedFile.OpenReadStream(),
                uploadedFile.ContentType, (ImagingDocumentCategory)category, 
                docId, batch, DateTime.Now);
            return new StatusCodeResult(200);
        }

        /// <summary>
        /// Upload multiple documents to imaging
        /// </summary>
        /// <param name="uploadedFiles">Files to upload</param>
        /// <param name="docType">doctype to upload as</param>
        /// <param name="category">ImagingDocumentCategory cast as an int</param>
        /// <param name="batch">Any meaningful string to identify batch</param>
        /// <returns></returns>
        [HttpPost("UploadDocument/{docType}")]
        public async Task<StatusCodeResult> UploadDocuments(IFormFileCollection uploadedFiles, string docType, int category, string batch)
        {
            //Pre-validate
            var unsupportedFiles = uploadedFiles
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
                var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
                await Parallel.ForEachAsync(uploadedFiles, options, async (uf, ct) => await UploadDocument(uf, docType, category, batch));
                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Error uploading files to imaging", category: "Imaging");
                return new StatusCodeResult(500);
                //TODO: remove throw when unhandled exceptions are handled.
                throw;
            }
        }
    }
}
