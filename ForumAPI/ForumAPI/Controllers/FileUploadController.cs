using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly FileUploadService fileUploadService;

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
        [ApiExplorerSettings(IgnoreApi = true)] // Swagger ignore this endpoint (Optional based on your needs)
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                // Call the service to upload the file and get the file name.
                string fileName = await fileUploadService.UploadFileAsync(file);

                // Return a success message along with the file name
                return Ok(new { Message = "File uploaded successfully!", FileName = fileName });
            }
            catch (Exception ex)
            {
                // Return an error message if file upload fails
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}