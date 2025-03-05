using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDAO service;

        public CategoryController(CategoryDAO service)
        {
            this.service = service;
        }

        [HttpGet]
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

        [HttpGet("{description}")]
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

        [HttpPost]
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

        [HttpDelete("{description}")]
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
