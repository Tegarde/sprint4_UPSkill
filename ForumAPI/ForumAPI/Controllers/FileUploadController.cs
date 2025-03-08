using ForumAPI.Services;
using Microsoft.AspNetCore.Http;
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
        /// Upload de um ficheiro único
        /// </summary>
        [HttpPost("upload")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadFile( IFormFile file)
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
