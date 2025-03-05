using ForumAPI.DTOs;
using ForumAPI.DTOs.CategoryDTOs;

namespace ForumAPI.Interfaces
{
    public interface CategoryDAO
    {
        public Task<List<CategoryDTO>> GetAllCategories();

        public Task<CategoryDTO> GetCategoryByDescription(string description);

        public Task DeleteCategory(string description);

        public Task<ResponseMessage> CreateCategory(CategoryDTO categoryDTO);
    }
}
