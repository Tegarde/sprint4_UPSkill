using ForumAPI.DTOs;

namespace ForumAPI.Interfaces
{
    public interface GreenitorDAO
    {
        public ResponseMessage RegisterUser(RegisterUserDTO registerUserDTO);
    }
}
