package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.springframework.web.multipart.MultipartFile;

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
