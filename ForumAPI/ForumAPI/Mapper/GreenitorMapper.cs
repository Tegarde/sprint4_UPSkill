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
                Badges = dto.Badges
            };
        }
    }
}
