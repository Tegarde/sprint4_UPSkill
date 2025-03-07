package pt.upskill.clientapi;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import pt.upskill.clientapi.JPARepositories.CategoryRepository;
import pt.upskill.clientapi.Models.Category;
import pt.upskill.clientapi.Services.CategoryService;
import pt.upskill.clientapi.CustomExceptions.CategoryNotFoundException;

import java.util.Arrays;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

public class CategoryServiceTest {

    @Mock
    private CategoryRepository categoryRepository;

    @InjectMocks
    private CategoryService categoryService;

    /**
     * Set up the test fixture. This is called before each test method.
     * <p>
     * Initializes mocks with MockitoAnnotations.
     */
    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    /**
     * Verifies that a category can be successfully created.
     * <p>
     * Preconditions: No category with the same description exists.
     * Postconditions: The category is created and returned.
     * <p>
     * Covers: {@link CategoryService#createCategory(Category)}
     */
    @Test
    void testCreateCategory_Success() {
        Category category = new Category("New Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(false);
        when(categoryRepository.save(category)).thenReturn(category);

        Category result = categoryService.createCategory(category);

        assertNotNull(result);
        assertEquals(category.getDescription(), result.getDescription());
    }

    /**
     * Verifies that attempting to create a category with an existing description fails.
     * <p>
     * Preconditions: A category with the same description exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link CategoryService#createCategory(Category)}
     */
    @Test
    void testCreateCategory_AlreadyExists() {
        Category category = new Category("Existing Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            categoryService.createCategory(category);
        });

        assertEquals("Category with description Existing Category already exists", exception.getMessage());
    }

    /**
     * Verifies that a category can be successfully retrieved by its description.
     * <p>
     * Preconditions: A category with the specified description exists.
     * Postconditions: The category is returned.
     * <p>
     * Covers: {@link CategoryService#getCategoryByDescription(String)}
     */
    @Test
    void testGetCategoryByDescription_Success() {
        Category category = new Category("Existing Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(true);
        when(categoryRepository.findByDescription(category.getDescription())).thenReturn(category);

        Category result = categoryService.getCategoryByDescription(category.getDescription());

        assertNotNull(result);
        assertEquals(category.getDescription(), result.getDescription());
    }

    /**
     * Verifies that attempting to retrieve a category by its description fails when the category does not exist.
     * <p>
     * Preconditions: No category with the specified description exists.
     * Postconditions: A CategoryNotFoundException is thrown.
     * <p>
     * Covers: {@link CategoryService#getCategoryByDescription(String)}
     */
    @Test
    void testGetCategoryByDescription_NotFound() {
        String description = "Nonexistent Category";

        when(categoryRepository.existsByDescription(description)).thenReturn(false);

        CategoryNotFoundException exception = assertThrows(CategoryNotFoundException.class, () -> {
            categoryService.getCategoryByDescription(description);
        });

        assertEquals("Category with description Nonexistent Category does not exist", exception.getMessage());
    }

    /**
     * Verifies that all categories can be successfully retrieved.
     * <p>
     * Preconditions: Two categories exist in the repository.
     * Postconditions: A list containing the existing categories is returned.
     * <p>
     * Covers: {@link CategoryService#getAllCategories()}
     */
    @Test
    void testGetAllCategories() {
        Category category1 = new Category("Category 1");
        Category category2 = new Category("Category 2");

        when(categoryRepository.findAll()).thenReturn(Arrays.asList(category1, category2));

        List<Category> result = categoryService.getAllCategories();

        assertNotNull(result);
        assertEquals(2, result.size());
        assertEquals("Category 1", result.get(0).getDescription());
        assertEquals("Category 2", result.get(1).getDescription());
    }

    /**
     * Verifies that a category can be successfully deleted by its description.
     * <p>
     * Preconditions: A category with the specified description exists in the repository.
     * Postconditions: The category with the specified description is deleted.
     * <p>
     * Covers: {@link CategoryService#deleteCategoryByDescription(String)}
     */
    @Test
    void testDeleteCategoryByDescription_Success() {
        Category category = new Category("Existing Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(true);
        when(categoryRepository.findByDescription(category.getDescription())).thenReturn(category);

        assertDoesNotThrow(() -> {
            categoryService.deleteCategoryByDescription(category.getDescription());
        });

        verify(categoryRepository, times(1)).delete(category);
    }

    /**
     * Verifies that attempting to delete a category by its description fails when the category does not exist.
     * <p>
     * Preconditions: No category with the specified description exists in the repository.
     * Postconditions: A CategoryNotFoundException is thrown.
     * <p>
     * Covers: {@link CategoryService#deleteCategoryByDescription(String)}
     */
    @Test
    void testDeleteCategoryByDescription_NotFound() {
        String description = "Nonexistent Category";

        when(categoryRepository.existsByDescription(description)).thenReturn(false);

        CategoryNotFoundException exception = assertThrows(CategoryNotFoundException.class, () -> {
            categoryService.deleteCategoryByDescription(description);
        });

        assertEquals("Category with description Nonexistent Category does not exist", exception.getMessage());
    }
}
