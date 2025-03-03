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
            if (request.IsSuccessStatusCode)
            {
                return await request.Content.ReadFromJsonAsync<ResponseMessage>();
            } else
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }
    }
}
