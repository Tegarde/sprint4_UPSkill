using ForumAPI.DTOs;
using ForumAPI.DTOs.CategoryDTOs;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface that defines the data access operations for categories.
    /// </summary>
    public interface CategoryDAO
    {
        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="CategoryDTO"/> as the result.</returns>
        public Task<List<CategoryDTO>> GetAllCategories();

        /// <summary>
        /// Retrieves a category by its description.
        /// </summary>
        /// <param name="description">The description of the category to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="CategoryDTO"/> as the result.</returns>
        public Task<CategoryDTO> GetCategoryByDescription(string description);

        /// <summary>
        /// Deletes a category based on its description.
        /// </summary>
        /// <param name="description">The description of the category to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteCategory(string description);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDTO">The category data transfer object containing the details of the category to be created.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="ResponseMessage"/> as the result.</returns>
        public Task<ResponseMessage> CreateCategory(CategoryDTO categoryDTO);
    }
}