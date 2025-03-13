using ForumAPI.DTOs;
using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.Interfaces;
using System.Net;

namespace ForumAPI.Services
{
    /// <summary>
    /// HTTP client for interacting with the category API.
    /// </summary>
    public class CategoryClient : CategoryDAO
    {
        private readonly HttpClient client;

        private readonly string BaseUrl = "http://localhost:8080/api/category";

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryClient"/> class.
        /// </summary>
        public CategoryClient()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryClient"/> class.
        /// </summary>
        /// <param name="client"></param>
        public CategoryClient(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Creates a new category in the API.
        /// </summary>
        /// <param name="categoryDTO">The category data to be created.</param>
        /// <returns>A response message indicating success or failure.</returns>
        /// <exception cref="ResponseStatusException">Thrown if the creation fails.</exception>
        public async Task<ResponseMessage> CreateCategory(CategoryDTO categoryDTO)
        {
            var request = await client.PostAsJsonAsync(BaseUrl, categoryDTO);
            var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
            if (request.IsSuccessStatusCode)
            {
                return message!;
            }
            else
            {
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        /// <summary>
        /// Deletes a category based on its description.
        /// </summary>
        /// <param name="description">The description of the category to be removed.</param>
        /// <exception cref="ResponseStatusException">Thrown if the deletion fails.</exception>
        public async Task DeleteCategory(string description)
        {
            var request = await client.DeleteAsync($"{BaseUrl}/{description}");
            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        /// <summary>
        /// Retrieves a category by its description.
        /// </summary>
        /// <param name="description">The description of the category.</param>
        /// <returns>The details of the found category.</returns>
        /// <exception cref="ResponseStatusException">Thrown if the category is not found.</exception>
        public async Task<CategoryDTO> GetCategoryByDescription(string description)
        {
            var request = await client.GetAsync($"{BaseUrl}/{description}");
            if (request.IsSuccessStatusCode)
            {
                var category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                return category!;
            }
            else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        /// <summary>
        /// Retrieves all available categories from the API.
        /// </summary>
        /// <returns>A list of categories or an empty list if no categories exist.</returns>
        /// <exception cref="ResponseStatusException">Thrown in case of an unexpected error.</exception>
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var request = await client.GetAsync(BaseUrl);
            if (request.StatusCode == HttpStatusCode.OK)
            {
                var categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                return categories!;
            }
            else if (request.StatusCode == HttpStatusCode.NoContent)
            {
                return new List<CategoryDTO>();
            }
            else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }
    }
}
