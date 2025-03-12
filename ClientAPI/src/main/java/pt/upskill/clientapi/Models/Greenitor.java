package pt.upskill.clientapi.Models;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Check;

import java.util.List;

/**
 * Represents a user (Greenitor) in the system.
 * A Greenitor is a user who interacts with the application and earns badges based on interactions.
 * The Greenitor entity contains user details like username, email, password, role, image, interactions, and badges.
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Greenitor {

    /**
     * Id of the Greenitor.
     * The primary key for the Greenitor entity, automatically generated.
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Username of the Greenitor.
     * A unique identifier for the Greenitor within the system.
     * This field is mandatory (cannot be null) and must be unique.
     */
    @Column(length = 50, nullable = false, unique = true)
    private String username;

    /**
     * Email of the Greenitor.
     * The email address of the Greenitor.
     * This field is mandatory (cannot be null) and must be unique.
     */
    @Column(length = 100, nullable = false, unique = true)
    private String email;

    /**
     * Password of the Greenitor.
     * The password for the Greenitor to log in to the system.
     * This field is mandatory (cannot be null).
     */
    @Column(length = 25, nullable = false)
    private String password;

    /**
     * Role of the Greenitor.
     * The role defines the permissions and responsibilities of the Greenitor within the application.
     * By default, the role is set to "User".
     */
    @Column(length = 50, nullable = false)
    private String role;

    /**
     * Image of the Greenitor.
     * An optional image representing the Greenitor, usually used for profile display.
     */
    private String image;

    /**
     * Interactions made by the Greenitor.
     * This field represents the number of interactions (like activities or tasks) performed by the Greenitor.
     * The value must be zero or positive.
     */
    @Check(constraints = "interactions >= 0")
    private int interactions;

    /**
     * Badges of the Greenitor.
     * A list of badges earned by the Greenitor based on their interactions.
     */
    @ManyToMany
    private List<Badge> badges;

    /**
     * Default role for a Greenitor.
     * This is set to "User" when no specific role is provided.
     */
    private static final String DEFAULT_ROLE = "User";

    /**
     * Constructor to create a new Greenitor without an image.
     * By default, the Greenitor will have the "User" role and zero interactions.
     *
     * @param username The username of the Greenitor.
     * @param email The email of the Greenitor.
     * @param password The password of the Greenitor.
     */
    public Greenitor(String username, String email, String password) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.role = DEFAULT_ROLE;
        this.interactions = 0;
    }

    /**
     * Constructor to create a new Greenitor with an image.
     * By default, the Greenitor will have the "User" role and zero interactions.
     *
     * @param username The username of the Greenitor.
     * @param email The email of the Greenitor.
     * @param password The password of the Greenitor.
     * @param image The image of the Greenitor.
     */
    public Greenitor(String username, String email, String password, String image) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.image = image;
        this.role = DEFAULT_ROLE;
        this.interactions = 0;
    }

    /**
     * Adds a badge to the Greenitor's list of badges.
     *
     * @param badge The badge to be added.
     */
    public void addBadge(Badge badge) {
        this.badges.add(badge);
    }

    /**
     * Removes a badge from the Greenitor's list of badges.
     *
     * @param badge The badge to be removed.
     */
    public void removeBadge(Badge badge) {
        this.badges.remove(badge);
    }
}