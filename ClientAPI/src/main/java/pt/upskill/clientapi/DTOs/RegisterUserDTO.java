package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object for registering a new user
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class RegisterUserDTO {

    /**
     * Username to be registered
     */
    private String username;

    /**
     * Email to be registered
     */
    private String email;

    /**
     * Password to be registered
     */
    private String password;

    /**
     * User image to be registered
     */
    private String image;

}
