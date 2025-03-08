using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;

namespace ForumAPI.Interfaces
{
    public interface GreenitorDAO
    {
        public Task<TokenDTO> Login(LoginDTO loginDTO);

        public Task<ResponseMessage> RegisterUser(RegisterUserWithImageDTO registerUserDTO);

        public Task<GreenitorDTO> GetUserByUsername(string username);

        Task IncrementUserInteractions(string username);

        Task DecrementUserInteractions(string username);
    }
}
