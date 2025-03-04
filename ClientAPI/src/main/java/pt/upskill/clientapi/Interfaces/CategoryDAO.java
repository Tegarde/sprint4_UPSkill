package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.Models.Category;

import java.util.List;

/**
 * Data Access Object for the Category entity
 */
public interface CategoryDAO {
    Category createCategory(Category category);
    Category getCategoryByDescription(String description);

    List<Category> getAllCategories();

    void deleteCategoryByDescription(String description);
}
