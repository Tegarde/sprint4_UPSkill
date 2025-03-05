package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data transfer object for badges
 */
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
public class BadgeDTO {

    /**
     * Description of the badge
     */
    private String description;

    /**
     * Interactions needed for unlocking the badge
     */
    private int interactions;
}
