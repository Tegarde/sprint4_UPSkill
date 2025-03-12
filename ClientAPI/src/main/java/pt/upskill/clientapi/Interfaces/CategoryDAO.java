package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.Models.Category;

import java.util.List;

/**
 * Data Access Object (DAO) interface for managing Category entities.
 * Provides methods to create, retrieve, and delete categories.
 */
public interface CategoryDAO {

    /**
     * Creates a new category in the system.
     *
     * @param category The category to be created.
     * @return The created category with any generated properties (e.g., ID).
     */
    Category createCategory(Category category);

    /**
     * Retrieves a category by its description.
     *
     * @param description The description of the category to retrieve.
     * @return The category with the specified description, or null if not found.
     */
    Category getCategoryByDescription(String description);

    /**
     * Retrieves a list of all categories in the system.
     *
     * @return A list of all categories.
     */
    List<Category> getAllCategories();

    /**
     * Deletes a category based on its description.
     *
     * @param description The description of the category to be deleted.
     */
    void deleteCategoryByDescription(String description);
}