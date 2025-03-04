package pt.upskill.clientapi.Models;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents a category.
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Category {

    /**
     * Id of the category.
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Description of the category.
     */
    @Column(length = 50, nullable = false, unique = true)
    private String description;

    public Category(String description) {
        this.description = description;
    }
}
