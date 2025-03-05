using ForumAPI.DTOs.BadgeDTOs;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    public class GreenitorDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public int Interections { get; set; }

        public List<BadgeDescriptionDTO> Badges { get; set; }

        public GreenitorDTO(string username, string email, int interections)
        {
            Username = username;
            Email = email;
            Interections = interections;
        }
    }
}
