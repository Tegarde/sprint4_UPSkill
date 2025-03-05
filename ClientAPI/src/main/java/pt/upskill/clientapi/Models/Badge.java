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
 * Represents a badge of the application
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Badge {

    /**
     * Id of the badge
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Description of the badge
     */
    @Column(length = 100, nullable = false)
    private String description;

    /**
     * Interactions needed to acheive the badge
     */
    @Check(constraints = "interactions >= 0")
    private int interactions;

    public Badge(String description, int interactions) {
        this.description = description;
        this.interactions = interactions;
    }
}
