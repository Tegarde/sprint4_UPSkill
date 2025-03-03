using ForumAPI.DTOs;
using ForumAPI.Interfaces;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace ForumAPI.Services
{
    public class GreenitorClient : GreenitorDAO
    {
        private static HttpClient client;

        private static readonly string BaseUrl = "http://localhost:8080/api/greenitor";

        public GreenitorClient()
        {
            client = new HttpClient();
        }

        public async Task<ResponseMessage> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            var request = await client.PostAsJsonAsync(BaseUrl, registerUserDTO);
            var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
            if (request.IsSuccessStatusCode)
            {
                return message!;
            } else
            { 
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        public async Task<TokenDTO> Login(LoginDTO loginDTO)
        {
            var request = await client.PostAsJsonAsync($"{BaseUrl}/login",loginDTO);
            if(request.IsSuccessStatusCode)
            {
                var token = await request.Content.ReadFromJsonAsync<TokenDTO>();
                return token!;
            }else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }

        }



    }
}
