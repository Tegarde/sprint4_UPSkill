package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

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
     * Role of the Greenitor
     */
    private String role;

    /**
     * Interactions of the Greenitor
     */
    private int interactions;

    /**
     * Badges achieved by the Greenitor
     */
    private List<BadgeDescriptionDTO> badges;
}
