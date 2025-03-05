using ForumAPI.DTOs;
using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.Interfaces;
using System.Net;

namespace ForumAPI.Services
{
    public class CategoryClient : CategoryDAO
    {
        private readonly HttpClient client;

        private readonly string BaseUrl = "http://localhost:8080/api/category";

        public CategoryClient()
        {
            this.client = new HttpClient();
        }

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

        public async Task DeleteCategory(string description)
        {
            var request = await client.DeleteAsync($"{BaseUrl}/{description}");
            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

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

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var request = await client.GetAsync(BaseUrl);
            if (request.StatusCode == HttpStatusCode.OK)
            {
                var categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                return categories!;
            } else if (request.StatusCode == HttpStatusCode.NoContent)
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
