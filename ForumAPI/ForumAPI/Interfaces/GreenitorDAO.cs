using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface defining operations related to Greenitor (user) management.
    /// </summary>
    public interface GreenitorDAO
    {
        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="loginDTO">The login credentials of the user.</param>
        /// <returns>A token if authentication is successful.</returns>
        public Task<TokenDTO> Login(LoginDTO loginDTO);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerUserDTO">The user data, including optional profile image.</param>
        /// <returns>A response message indicating success or failure.</returns>
        public Task<ResponseMessage> RegisterUser(RegisterUserWithImageDTO registerUserDTO);

        /// <summary>
        /// Retrieves user details based on the username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>User details if found.</returns>
        public Task<GreenitorDTO> GetUserByUsername(string username);

        /// <summary>
        /// Increments the interaction count of a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        Task IncrementUserInteractions(string username);

        /// <summary>
        /// Decrements the interaction count of a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        Task DecrementUserInteractions(string username);

        /// <summary>
        /// Retrieves a list of all users without role information.
        /// </summary>
        /// <returns>A list of users.</returns>
        Task<List<GreenitorWithoutRoleDTO>> GetAllGreenitors();

        /// <summary>
        /// Updates a user's profile.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="userDTO">The updated user data.</param>
        /// <returns>A response message indicating success or failure.</returns>
        Task<ResponseMessage> UpdateUserProfile(string username, UpdateUserDTO userDTO);
    }
}