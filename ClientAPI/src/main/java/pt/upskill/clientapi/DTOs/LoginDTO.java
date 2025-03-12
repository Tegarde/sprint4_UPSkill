package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents a login data transfer object.
 * This class is used to hold the login credentials (email and password) of a user.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class LoginDTO {

    /**
     * The email of the user.
     * This field is used to identify the user during the login process.
     */
    private String email;

    /**
     * The password of the user.
     * This field is used to authenticate the user during the login process.
     */
    private String password;

}