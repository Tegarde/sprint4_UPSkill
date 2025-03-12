using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace ForumAPI.Services
{
    public class GreenitorClient : GreenitorDAO
    {
        private readonly HttpClient client;
        private static readonly string BaseUrl = "http://localhost:8080/api/greenitor";

        public GreenitorClient()
        {
            client = new HttpClient();
        }

        public async Task<ResponseMessage> RegisterUser(RegisterUserWithImageDTO registerUserDTO)
        {
            var request = await client.PostAsJsonAsync(BaseUrl, registerUserDTO);
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

        public async Task<TokenDTO> Login(LoginDTO loginDTO)
        {
            var request = await client.PostAsJsonAsync($"{BaseUrl}/login", loginDTO);
            if (request.IsSuccessStatusCode)
            {
                var token = await request.Content.ReadFromJsonAsync<TokenDTO>();
                return token!;
            }
            else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        public async Task<GreenitorDTO> GetUserByUsername(string username)
        {
            var request = await client.GetAsync($"{BaseUrl}/user/{username}");

            if (request.IsSuccessStatusCode)
            {
                var user = await request.Content.ReadFromJsonAsync<GreenitorDTO>();

                return user!;
            }
            else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();

                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        public async Task IncrementUserInteractions(string username)
        {
            var request = await client.PatchAsync($"{BaseUrl}/increment/{username}", null);

            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        public async Task DecrementUserInteractions(string username)
        {
            var request = await client.PatchAsync($"{BaseUrl}/decrement/{username}", null);

            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        public async Task<List<GreenitorWithoutRoleDTO>> GetAllGreenitors()
        {
            var request = await client.GetAsync($"{BaseUrl}");
            var greenitors = await request.Content.ReadFromJsonAsync<List<GreenitorWithoutRoleDTO>>();
            return greenitors!;
        }
        public async Task<ResponseMessage> UpdateUserProfile(string username, UpdateUserDTO userDTO)
        {
            
            var request = await client.PutAsJsonAsync($"{BaseUrl}/{username}", userDTO);
            var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
            if (!request.IsSuccessStatusCode)
            {
                
                throw new ResponseStatusException(request.StatusCode, message!);
            }
            return message!;
        }
    }
}
