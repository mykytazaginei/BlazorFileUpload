using System.Net;
using BlazorFileUpload.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFileUpload.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FIleController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FIleController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
        {

            List<UploadResult> uploadResults = new List<UploadResult>();
             
            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlDecode(untrustedFileName);

                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

                await using FileStream fs = new FileStream(path, FileMode.Open);
                await fs.CopyToAsync(fs);

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResults.Add(uploadResult);

            }

            return Ok(uploadResults);

        }
    }
}
