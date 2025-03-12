package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object for registering a new user.
 * This class is used to hold the necessary information for registering a new user.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class RegisterUserDTO {

    /**
     * The username of the user to be registered.
     * This will be used as the unique identifier for the user.
     */
    private String username;

    /**
     * The email of the user to be registered.
     * This will be used for communication and possibly authentication.
     */
    private String email;

    /**
     * The password of the user to be registered.
     * This is used for authenticating the user during login.
     */
    private String password;

    /**
     * The image URL or path associated with the user to be registered.
     * This can be a link to the user's avatar or profile image.
     */
    private String image;

}