package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.CategoryNotFoundException;
import pt.upskill.clientapi.Interfaces.CategoryDAO;
import pt.upskill.clientapi.JPARepositories.CategoryRepository;
import pt.upskill.clientapi.Models.Category;

import java.util.List;

@Service
public class CategoryService implements CategoryDAO {

    private final CategoryRepository repository;

    public CategoryService(CategoryRepository repository) {
        this.repository = repository;
    }
    @Override
    public Category createCategory(Category category) {
        if (repository.existsByDescription(category.getDescription())) {
            throw new IllegalArgumentException(String.format("Category with description %s already exists", category.getDescription()));
        }
        return repository.save(category);
    }

    @Override
    public Category getCategoryByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new CategoryNotFoundException(String.format("Category with description %s does not exist", description));
        }
        return repository.findByDescription(description);
    }

    @Override
    public List<Category> getAllCategories() {
        return repository.findAll();
    }

    @Override
    public void deleteCategoryByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new CategoryNotFoundException(String.format("Category with description %s does not exist", description));
        }
        repository.delete(repository.findByDescription(description));
    }
}
