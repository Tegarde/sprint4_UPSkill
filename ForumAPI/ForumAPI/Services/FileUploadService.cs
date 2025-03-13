namespace ForumAPI.Services
{
    /// <summary>
    /// Service for handling file uploads.
    /// </summary>
    public class FileUploadService
    {
        private readonly string uploadPath = "uploads/";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadService"/> class.
        /// Ensures the upload directory exists.
        /// </summary>
        public FileUploadService()
        {
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
        }

        /// <summary>
        /// Uploads a file asynchronously and stores it in the upload directory.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>The file path of the uploaded file.</returns>
        /// <exception cref="ArgumentException">Thrown if the file is invalid.</exception>
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file.");
            }

            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Returns only the file path to avoid exposing the full directory structure
        }
    }
}