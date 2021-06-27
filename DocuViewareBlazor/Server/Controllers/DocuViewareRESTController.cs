using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using GdPicture14.WEB;
using DocuViewareBlazor.Shared;
using Microsoft.AspNetCore.Hosting;

namespace DocuViewareBlazor.Server.Controllers
{
    public class DocuViewareRESTController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public DocuViewareRESTController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("api/DocuViewareREST/GetDocuViewareControl")]
        public IActionResult GetDocuViewareControl([FromBody] DocuViewareConfiguration controlConfiguration)
        {
            if (!DocuViewareManager.IsSessionAlive(controlConfiguration.SessionId))
            {
                if (!string.IsNullOrEmpty(controlConfiguration.SessionId) && !string.IsNullOrEmpty(controlConfiguration.ControlId))
                {
                    DocuViewareManager.CreateDocuViewareSession(controlConfiguration.SessionId,
                        controlConfiguration.ControlId, 20);
                }
                else
                {
                    throw new Exception("Invalid session identifier and/or invalid control identifier.");
                }
            }
            using var docuVieware = new DocuViewareControl(controlConfiguration.SessionId)
            {
                AllowPrint = controlConfiguration.AllowPrint,
                EnablePrintButton = controlConfiguration.EnablePrintButton,
                AllowUpload = controlConfiguration.AllowUpload,
                EnableFileUploadButton = controlConfiguration.EnableFileUploadButton,
                CollapsedSnapIn = controlConfiguration.CollapsedSnapIn,
                ShowAnnotationsSnapIn = controlConfiguration.ShowAnnotationsSnapIn,
                EnableRotateButtons = controlConfiguration.EnableRotateButtons,
                EnableZoomButtons = controlConfiguration.EnableZoomButtons,
                EnablePageViewButtons = controlConfiguration.EnablePageViewButtons,
                EnableMultipleThumbnailSelection = controlConfiguration.EnableMultipleThumbnailSelection,
                EnableMouseModeButtons = controlConfiguration.EnableMouseModeButtons,
                EnableFormFieldsEdition = controlConfiguration.EnableFormFieldsEdition,
                EnableTwainAcquisitionButton = controlConfiguration.EnableTwainAcquisitionButton,
                PageDisplayMode = GdPicture14.PageDisplayMode.SinglePageView,
                MaxUploadSize = 36700160, // 35MB
                Height = "600px",
                OpenZoomMode = GdPicture14.ViewerZoomMode.ZoomModeHeightViewer
            };
            using StringWriter controlOutput = new StringWriter();
            docuVieware.RenderControl(controlOutput);
            docuVieware.LoadFromFile(Path.Combine(_env.ContentRootPath, "Storage", "multipage_tiff_example.tif"));
            return new OkObjectResult(new DocuViewareRESTOutputResponse
            {
                HtmlContent = controlOutput.ToString()
            });
        }

        [HttpGet("api/DocuViewareREST/ping")]
        public string ping()
        {
            return "pong";
        }
        [HttpPost("api/DocuViewareREST/baserequest")]
        public string baserequest([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.baserequest(jsonString);
        }


        [HttpGet("api/DocuViewareREST/print")]
        public HttpResponseMessage Print(string sessionID, string pageRange, bool printAnnotations)
        {
            return DocuViewareControllerActionsHandler.print(sessionID, pageRange, printAnnotations);
        }

        [HttpGet("api/DocuViewareREST/save")]
        public IActionResult Save(string sessionID, string fileName, string format, string pageRange, bool dropAnnotations, bool flattenAnnotations)
        {
            DocuViewareControllerActionsHandler.save(sessionID, ref fileName, format, pageRange, dropAnnotations, flattenAnnotations, out HttpStatusCode statusCode, out string reasonPhrase, out byte[] content, out string contentType);
            if (statusCode == HttpStatusCode.OK)
            {
                return File(content, contentType, fileName);
            }
            else
            {
                return StatusCode((int)statusCode, reasonPhrase);
            }
        }


        [HttpGet("api/DocuViewareREST/twainservicesetupdownload")]
        public IActionResult TwainServiceSetupDownload(string sessionID)
        {
            DocuViewareControllerActionsHandler.twainservicesetupdownload(sessionID, out HttpStatusCode statusCode, out byte[] content, out string contentType, out string fileName, out string reasonPhrase);
            if (statusCode == HttpStatusCode.OK)
            {
                return File(content, contentType, fileName);
            }
            else
            {
                return StatusCode((int)statusCode, reasonPhrase);
            }
        }

        [HttpPost("api/DocuViewareREST/formfieldupdate")]
        public string FormfieldUpdate([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.formfieldupdate(jsonString);
        }

        [HttpPost("api/DocuViewareREST/annotupdate")]
        public string AnnotUpdate([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.annotupdate(jsonString);
        }

        [HttpPost("api/DocuViewareREST/loadfromfile")]
        public string LoadFromFile([FromBody] object jsonString)
        {
            return DocuViewareControllerActionsHandler.loadfromfile(jsonString);
        }

        [HttpPost("api/DocuViewareREST/loadfromfilemultipart")]
        public string LoadFromFileMultipart()
        {
            return DocuViewareControllerActionsHandler.loadfromfilemultipart(Request);
        }
    }
}
