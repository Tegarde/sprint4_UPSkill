using ForumAPI.DTOs.GreenitorDTOs;

namespace ForumAPI.Mapper
{
    /// <summary>
    /// A helper class to map between various Greenitor-related DTOs.
    /// </summary>
    public class GreenitorMapper
    {
        /// <summary>
        /// Converts a GreenitorDTO to a GreenitorWithoutRoleDTO.
        /// </summary>
        /// <param name="dto">The GreenitorDTO to be mapped.</param>
        /// <returns>A GreenitorWithoutRoleDTO with data from the provided GreenitorDTO.</returns>
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

        /// <summary>
        /// Converts a RegisterUserDTO to a RegisterUserWithImageDTO.
        /// </summary>
        /// <param name="dto">The RegisterUserDTO to be mapped.</param>
        /// <returns>A RegisterUserWithImageDTO with data from the provided RegisterUserDTO.</returns>
        public static RegisterUserWithImageDTO toRegisterUserWithImageDTO(RegisterUserDTO dto)
        {
            return new RegisterUserWithImageDTO
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                // Image is not set here as RegisterUserDTO does not include an image property.
            };
        }
    }
}