package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object for the Greenitor entity
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class GreenitorDTO {

    /**
     * Username of the Greenitor
     */
    private String username;

    /**
     * Email of the Greenitor
     */
    private String email;

    /**
     * Interactions of the Greenitor
     */
    private int interactions;
}
