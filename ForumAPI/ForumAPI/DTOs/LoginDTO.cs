using System.ComponentModel.DataAnnotations;

namespace ForumAPI.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public LoginDTO(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
