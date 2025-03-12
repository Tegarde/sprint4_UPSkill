package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object (DTO) for Badge Description.
 * This class represents the description and image associated with a badge.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class BadgeDescriptionDTO {

    /**
     * Description of the badge.
     * This field provides a textual representation of the badge's purpose or significance.
     */
    private String description;

    /**
     * Image associated with the badge.
     * This field contains the URL or file path to the image representing the badge.
     */
    private String image;
}