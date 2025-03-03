package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class LoginDTO {

    /**
     * Represents a login data transfer object.
     *
     * <p>
     * This class is a data transfer object, holding the email and password of a user.
     * </p>
     */


    /**
     * The email of the user.
     */
    private String email;

    /**
     * The password of the user.
     */
    private String password;

}
