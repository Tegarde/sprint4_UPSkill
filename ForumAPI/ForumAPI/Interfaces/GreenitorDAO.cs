using ForumAPI.DTOs;

namespace ForumAPI.Interfaces
{
    public interface GreenitorDAO
    {
        public Task<TokenDTO> Login(LoginDTO loginDTO);

        public Task<ResponseMessage> RegisterUser(RegisterUserDTO registerUserDTO);
    }
}
