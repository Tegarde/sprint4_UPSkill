package pt.upskill.clientapi.Models;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Check;

/**
 * Represents a badge in the application.
 * A badge is associated with a certain number of interactions that must be achieved by a Greenitor
 * in order to unlock the badge. It also contains a description and an optional image.
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Badge {

    /**
     * Id of the badge.
     * The primary key for the badge entity.
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Description of the badge.
     * This field holds a brief description of the badge.
     * The description is mandatory and cannot be null.
     */
    @Column(length = 100, nullable = false)
    private String description;

    /**
     * Interactions needed to achieve the badge.
     * Represents the number of interactions that a Greenitor must achieve to earn this badge.
     * The value must be non-negative.
     */
    @Check(constraints = "interactions >= 0")
    private int interactions;

    /**
     * The image associated with the badge.
     * This field can hold an image URL or path for the badge, if available.
     */
    private String image;

    /**
     * Constructor for creating a Badge with description and interactions.
     *
     * @param description The description of the badge.
     * @param interactions The number of interactions required to earn the badge.
     */
    public Badge(String description, int interactions) {
        this.description = description;
        this.interactions = interactions;
    }
}