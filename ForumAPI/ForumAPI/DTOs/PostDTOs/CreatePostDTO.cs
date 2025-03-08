using ForumAPI.DTOs.CategoryDTOs;

namespace ForumAPI.DTOs.PostDTOs
{
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public string Category { get; set; }

        public IFormFile? Image { get; set; }
    }
}
