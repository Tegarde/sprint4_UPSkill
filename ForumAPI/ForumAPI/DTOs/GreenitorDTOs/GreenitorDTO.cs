using ForumAPI.DTOs.BadgeDTOs;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    public class GreenitorDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public int Interactions { get; set; }

        public string Image { get; set; }

        public List<BadgeDescriptionDTO> Badges { get; set; }

        public GreenitorDTO(string username, string email, int interactions)
        {
            Username = username;
            Email = email;
            Interactions = interactions;
        }
    }
}
