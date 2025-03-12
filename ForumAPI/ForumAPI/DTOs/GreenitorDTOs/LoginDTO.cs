using System.ComponentModel.DataAnnotations;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login details, including email and password.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// This property is required for login.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// This property is required for login.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDTO"/> class with the specified email and password.
        /// </summary>
        /// <param name="Email">The email address of the user.</param>
        /// <param name="Password">The password of the user.</param>
        public LoginDTO(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}