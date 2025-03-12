package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.CategoryNotFoundException;
import pt.upskill.clientapi.Interfaces.CategoryDAO;
import pt.upskill.clientapi.JPARepositories.CategoryRepository;
import pt.upskill.clientapi.Models.Category;

import java.util.List;

/**
 * Service class for handling business logic related to categories.
 * This class implements the CategoryDAO interface and provides methods to create, retrieve, and delete categories.
 * It interacts with the CategoryRepository to perform database operations.
 */
@Service
public class CategoryService implements CategoryDAO {

    private final CategoryRepository repository;

    /**
     * Constructs a CategoryService with the provided CategoryRepository.
     *
     * @param repository the CategoryRepository used to interact with the database
     */
    public CategoryService(CategoryRepository repository) {
        this.repository = repository;
    }

    /**
     * Creates a new category.
     * The method checks if a category with the same description already exists.
     * If it exists, an IllegalArgumentException is thrown.
     *
     * @param category the Category object to be created
     * @return the created Category object
     * @throws IllegalArgumentException if a category with the same description already exists
     */
    @Override
    public Category createCategory(Category category) {
        if (repository.existsByDescription(category.getDescription())) {
            throw new IllegalArgumentException(String.format("Category with description %s already exists", category.getDescription()));
        }
        return repository.save(category);
    }

    /**
     * Retrieves a category by its description.
     * If no category with the given description exists, a CategoryNotFoundException is thrown.
     *
     * @param description the description of the category to be retrieved
     * @return the Category object with the given description
     * @throws CategoryNotFoundException if no category with the given description exists
     */
    @Override
    public Category getCategoryByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new CategoryNotFoundException(String.format("Category with description %s does not exist", description));
        }
        return repository.findByDescription(description);
    }

    /**
     * Retrieves all categories from the database.
     *
     * @return a list of all Category objects
     */
    @Override
    public List<Category> getAllCategories() {
        return repository.findAll();
    }

    /**
     * Deletes a category by its description.
     * If no category with the given description exists, a CategoryNotFoundException is thrown.
     *
     * @param description the description of the category to be deleted
     * @throws CategoryNotFoundException if no category with the given description exists
     */
    @Override
    public void deleteCategoryByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new CategoryNotFoundException(String.format("Category with description %s does not exist", description));
        }
        repository.delete(repository.findByDescription(description));
    }
}