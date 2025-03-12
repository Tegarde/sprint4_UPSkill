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
 * Represents a category in the application.
 * A category is a classification or grouping that can be assigned to badges, users, or other entities.
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Category {

    /**
     * Id of the category.
     * The primary key for the category entity, automatically generated.
     */
    @Id
    @GeneratedValue(strategy = jakarta.persistence.GenerationType.IDENTITY)
    private int id;

    /**
     * Description of the category.
     * A brief textual description that represents the category.
     * This field is mandatory (cannot be null), and must be unique for each category.
     */
    @Column(length = 50, nullable = false, unique = true)
    private String description;

    /**
     * Constructor for creating a Category with a description.
     *
     * @param description The description of the category.
     */
    public Category(String description) {
        this.description = description;
    }
}