package pt.upskill.clientapi.Models;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.Check;

import java.util.List;

/**
 * Represents a user in the system
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class User {

    /**
     * Id of the user
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Username of the user
     */
    @Column(length = 50, nullable = false, unique = true)
    private String username;

    /**
     * Email of the user
     */
    @Column(length = 100, nullable = false, unique = true)
    private String email;

    /**
     * Password of the user
     */
    @Check(constraints = "password.length() >= 6")
    @Column(length = 25, nullable = false)
    private String password;

    /**
     * Role of the user
     */
    @Column(length = 50, nullable = false)
    private String role;

    /**
     * Interactions made by the user
     */
    @Check(constraints = "interactions >= 0")
    private int interactions;

    /**
     * Badges of the user
     */
    @ManyToMany
    private List<Badge> badges;

    public User(String username, String email, String password) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.role = "User";
        this.interactions = 0;
    }
}
