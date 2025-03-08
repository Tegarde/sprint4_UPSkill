using ForumAPI.DTOs.GreenitorDTOs;

namespace ForumAPI.Mapper
{
    public class GreenitorMapper
    {
        public static GreenitorWithoutRoleDTO toGreenitorWithoutRoleDTO(GreenitorDTO dto)
        {
            return new GreenitorWithoutRoleDTO
            {
                Username = dto.Username,
                Email = dto.Email,
                Interactions = dto.Interactions,
                Badges = dto.Badges,
                Image = dto.Image
            };
        }

        public static RegisterUserWithImageDTO toRegisterUserWithImageDTO(RegisterUserDTO dto)
        {
            return new RegisterUserWithImageDTO
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
            };
        }

    }
}
