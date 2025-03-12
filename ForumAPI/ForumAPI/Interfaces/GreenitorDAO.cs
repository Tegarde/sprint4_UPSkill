using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface defining data access operations related to Greenitors (users).
    /// </summary>
    public interface GreenitorDAO
    {
        /// <summary>
        /// Authenticates a user and generates a token for login.
        /// </summary>
        /// <param name="loginDTO">The login data transfer object containing the user's credentials.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="TokenDTO"/> containing the login token.</returns>
        public Task<TokenDTO> Login(LoginDTO loginDTO);

        /// <summary>
        /// Registers a new user (Greenitor) in the system.
        /// </summary>
        /// <param name="registerUserDTO">The data transfer object containing the user's registration details, including their image.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="ResponseMessage"/> indicating success or failure.</returns>
        public Task<ResponseMessage> RegisterUser(RegisterUserWithImageDTO registerUserDTO);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="GreenitorDTO"/> containing the user's details.</returns>
        public Task<GreenitorDTO> GetUserByUsername(string username);

        /// <summary>
        /// Increments the interaction count for a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose interactions count is to be incremented.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task IncrementUserInteractions(string username);

        /// <summary>
        /// Decrements the interaction count for a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose interactions count is to be decremented.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DecrementUserInteractions(string username);

        /// <summary>
        /// Retrieves a list of all Greenitors (users) without their roles.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="GreenitorWithoutRoleDTO"/> representing all users.</returns>
        Task<List<GreenitorWithoutRoleDTO>> GetAllGreenitors();
    }
}