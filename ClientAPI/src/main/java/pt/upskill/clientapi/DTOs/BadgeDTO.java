package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object (DTO) for a Badge.
 * This class represents the data associated with a badge, including its description and the number of interactions required to unlock it.
 */
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
public class BadgeDTO {

    /**
     * Description of the badge.
     * This field provides a textual representation of what the badge signifies or its purpose.
     */
    private String description;

    /**
     * Interactions needed for unlocking the badge.
     * This field indicates how many interactions (e.g., user actions) are required to unlock or earn the badge.
     */
    private int interactions;
}