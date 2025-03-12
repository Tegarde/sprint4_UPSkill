using ForumAPI.DTOs.BadgeDTOs;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    public class GreenitorWithoutRoleDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public int? Interactions { get; set; }

        public string Image { get; set; }

        public List<BadgeDescriptionDTO> Badges { get; set; }
    }
}
