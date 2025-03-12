package pt.upskill.clientapi.Models;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents a Token for a Greenitor's session.
 * The Token holds the username and role of the Greenitor to authenticate and authorize access to the system.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Token {

    /**
     * The username of the Greenitor associated with this token.
     * It identifies the user in the system.
     */
    public String username;

    /**
     * The role of the Greenitor associated with this token.
     * It defines the level of access or permissions the Greenitor has within the system.
     */
    public String role;
}