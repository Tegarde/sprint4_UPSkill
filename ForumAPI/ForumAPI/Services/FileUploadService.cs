namespace ForumAPI.Services
{
    public class FileUploadService
    {

        private readonly string uploadPath = "uploads/";

        public FileUploadService()
        {
            if (!Directory.Exists(uploadPath))
            { 
                Directory.CreateDirectory(uploadPath); 
            }
            
        }

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

            return filePath; // Retorna apenas o nome do ficheiro para evitar expor o caminho completo
        }



    }
}
