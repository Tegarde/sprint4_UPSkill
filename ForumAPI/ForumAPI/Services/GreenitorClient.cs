using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;

namespace ForumAPI.Services
{
    /// <summary>
    /// HTTP client for interacting with the Greenitor API.
    /// </summary>
    public class GreenitorClient : GreenitorDAO
    {
        private readonly HttpClient client;
        private static readonly string BaseUrl = "http://localhost:8080/api/greenitor";

        /// <summary>
        /// Initializes a new instance of the <see cref="GreenitorClient"/> class.
        /// </summary>
        public GreenitorClient()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerUserDTO">The user data including an image.</param>
        /// <returns>A response message indicating success or failure.</returns>
        /// <exception cref="ResponseStatusException">Thrown if registration fails.</exception>
        public GreenitorClient(HttpClient client)
        {
            this.client = client;
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

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="loginDTO">The user's login credentials.</param>
        /// <returns>A token DTO containing authentication information.</returns>
        /// <exception cref="ResponseStatusException">Thrown if login fails.</exception>
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

        /// <summary>
        /// Retrieves user details by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user's details.</returns>
        /// <exception cref="ResponseStatusException">Thrown if the user is not found.</exception>
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

        /// <summary>
        /// Increments a user's interaction count.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <exception cref="ResponseStatusException">Thrown if the operation fails.</exception>
        public async Task IncrementUserInteractions(string username)
        {
            var request = await client.PatchAsync($"{BaseUrl}/increment/{username}", null);

            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        /// <summary>
        /// Decrements a user's interaction count.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <exception cref="ResponseStatusException">Thrown if the operation fails.</exception>
        public async Task DecrementUserInteractions(string username)
        {
            var request = await client.PatchAsync($"{BaseUrl}/decrement/{username}", null);

            if (!request.IsSuccessStatusCode)
            {
                var message = await request.Content.ReadFromJsonAsync<ResponseMessage>();
                throw new ResponseStatusException(request.StatusCode, message!);
            }
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>A list of users without roles.</returns>
        public async Task<List<GreenitorWithoutRoleDTO>> GetAllGreenitors()
        {
            var request = await client.GetAsync($"{BaseUrl}");
            var greenitors = await request.Content.ReadFromJsonAsync<List<GreenitorWithoutRoleDTO>>();
            return greenitors!;
        }

        /// <summary>
        /// Updates a user's profile.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="userDTO">The updated user data.</param>
        /// <returns>A response message indicating success or failure.</returns>
        /// <exception cref="ResponseStatusException">Thrown if the update fails.</exception>
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