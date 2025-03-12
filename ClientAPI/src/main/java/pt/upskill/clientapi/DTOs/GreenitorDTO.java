package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

/**
 * Data Transfer Object (DTO) for the Greenitor entity.
 * This class represents the data associated with a Greenitor, including their username, email, role, interactions, image, and badges.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class GreenitorDTO {

    /**
     * Username of the Greenitor.
     * This field stores the unique identifier for the Greenitor.
     */
    private String username;

    /**
     * Email of the Greenitor.
     * This field contains the email address associated with the Greenitor account.
     */
    private String email;

    /**
     * Role of the Greenitor.
     * This field defines the role (e.g., "Admin", "User") of the Greenitor within the application.
     */
    private String role;

    /**
     * Interactions of the Greenitor.
     * This field holds the number of interactions the Greenitor has made within the system.
     */
    private int interactions;

    /**
     * Image of the Greenitor.
     * This field contains the URL or path to the Greenitor's profile image.
     */
    private String image;

    /**
     * Badges achieved by the Greenitor.
     * This field holds a list of badges the Greenitor has earned, represented as BadgeDescriptionDTO objects.
     */
    private List<BadgeDescriptionDTO> badges;
}