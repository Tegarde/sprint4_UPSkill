using ForumAPI.DTOs;

namespace ForumAPI.Interfaces
{
    public interface GreenitorDAO
    {
        public Task<ResponseMessage> RegisterUser(RegisterUserDTO registerUserDTO);
    }
}
