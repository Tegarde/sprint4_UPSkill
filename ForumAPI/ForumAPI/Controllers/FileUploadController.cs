using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ForumAPI.Controllers
{
    /// <summary>
    /// Controller for handling file uploads.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("File Upload Management")]
    public class FileUploadController : ControllerBase
    {
        private readonly FileUploadService fileUploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadController"/> class.
        /// </summary>
        /// <param name="fileUploadService">The service responsible for handling file uploads.</param>
        public FileUploadController(FileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Uploads a single file to the server.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>A success message with the uploaded file's name if successful.</returns>
        /// <response code="200">Returns a success message and the uploaded file's name.</response>
        /// <response code="400">If there is an error with the file upload process.</response>
        [HttpPost("upload")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                string fileName = await fileUploadService.UploadFileAsync(file);

                return Ok(new { Message = "File uploaded successfully!", FileName = fileName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}