package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

/**
 * Data Transfer Object (DTO) for searching Greenitors.
 * This class contains essential information about a Greenitor that may be returned during a search operation,
 * such as their username, email, image, and badges.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class GreenitorSearchDTO {

    /**
     * Username of the Greenitor.
     * This field holds the unique identifier for the Greenitor in the system.
     */
    public String username;

    /**
     * Email of the Greenitor.
     * This field holds the Greenitor's email address.
     */
    public String email;

    /**
     * Image of the Greenitor.
     * This field stores the URL or path to the Greenitor's profile image.
     */
    public String image;


    public int interactions;

    /**
     * List of badges associated with the Greenitor.
     * This field contains the badges that the Greenitor has earned, represented by BadgeDescriptionDTO objects.
     */

    private List<BadgeDescriptionDTO> badges;
}
