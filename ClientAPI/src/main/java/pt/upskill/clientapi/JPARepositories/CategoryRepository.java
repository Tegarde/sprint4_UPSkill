package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Category;

/**
 * Repository interface for managing Category entities.
 * Provides methods for querying the Category table in the database.
 */
@Repository
public interface CategoryRepository extends JpaRepository<Category, Integer> {

    /**
     * Checks if a category with the specified description exists.
     *
     * @param description The description of the category to check.
     * @return true if a category with the given description exists, otherwise false.
     */
    boolean existsByDescription(String description);

    /**
     * Retrieves a category by its description.
     *
     * @param description The description of the category to retrieve.
     * @return The Category object corresponding to the given description.
     */
    Category findByDescription(String description);
}