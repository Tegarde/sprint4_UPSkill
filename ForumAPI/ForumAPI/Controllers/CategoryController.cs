using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.Interfaces;

namespace ForumAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Category Management")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDAO service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="service">The category service.</param>
        public CategoryController(CategoryDAO service)
        {
            this.service = service;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of all categories available in the system.
        /// If no categories exist, a 204 No Content status will be returned.
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/> containing the list of categories or a No Content status.</returns>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve all categories",
            Description = "This endpoint fetches all categories from the database. Returns a 204 No Content if no categories are found."
        )]
        [SwaggerResponse(200, "List of categories returned successfully.")]
        [SwaggerResponse(204, "No categories found.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await service.GetAllCategories();
                return (categories.Any()) ? Ok(categories) : NoContent();
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves a category by its description.
        /// </summary>
        /// <param name="description">The description of the category to retrieve.</param>
        /// <remarks>
        /// This endpoint allows the retrieval of a category based on its description.
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/> containing the category details.</returns>
        [HttpGet("{description}")]
        [SwaggerOperation(
            Summary = "Retrieve category by description",
            Description = "This endpoint retrieves a category by its description."
        )]
        [SwaggerResponse(200, "Category found successfully.")]
        [SwaggerResponse(400, "Bad request.")]
        [SwaggerResponse(404, "Category not found.")]
        public async Task<IActionResult> GetCategoryByDescription(string description)
        {
            try
            {
                var category = await service.GetCategoryByDescription(description);
                return Ok(category);
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category data to be created.</param>
        /// <remarks>
        /// This endpoint allows the creation of a new category.
        /// A successful creation returns a 201 Created status with the created category.
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/> indicating the outcome of the category creation.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new category",
            Description = "This endpoint creates a new category in the database."
        )]
        [SwaggerResponse(201, "Category created successfully.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO category)
        {
            try
            {
                var message = await service.CreateCategory(category);
                return CreatedAtAction(nameof(CreateCategory), message);
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes a category by its description.
        /// </summary>
        /// <param name="description">The description of the category to delete.</param>
        /// <remarks>
        /// This endpoint deletes a category based on its description. A successful deletion returns a 204 No Content status.
        /// </remarks>
        /// <returns>An <see cref="IActionResult"/> indicating the outcome of the category deletion.</returns>
        [HttpDelete("{description}")]
        [SwaggerOperation(
            Summary = "Delete category by description",
            Description = "This endpoint deletes a category from the database based on its description."
        )]
        [SwaggerResponse(204, "Category deleted successfully.")]
        [SwaggerResponse(400, "Bad request.")]
        [SwaggerResponse(404, "Category not found.")]
        public async Task<IActionResult> DeleteCategory(string description)
        {
            try
            {
                await service.DeleteCategory(description);
                return NoContent();
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "Something went wrong");
            }
        }
    }
}
