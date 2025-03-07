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

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    @Test
    void testCreateCategory_Success() {
        Category category = new Category("New Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(false);
        when(categoryRepository.save(category)).thenReturn(category);

        Category result = categoryService.createCategory(category);

        assertNotNull(result);
        assertEquals(category.getDescription(), result.getDescription());
    }

    @Test
    void testCreateCategory_AlreadyExists() {
        Category category = new Category("Existing Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            categoryService.createCategory(category);
        });

        assertEquals("Category with description Existing Category already exists", exception.getMessage());
    }

    @Test
    void testGetCategoryByDescription_Success() {
        Category category = new Category("Existing Category");

        when(categoryRepository.existsByDescription(category.getDescription())).thenReturn(true);
        when(categoryRepository.findByDescription(category.getDescription())).thenReturn(category);

        Category result = categoryService.getCategoryByDescription(category.getDescription());

        assertNotNull(result);
        assertEquals(category.getDescription(), result.getDescription());
    }

    @Test
    void testGetCategoryByDescription_NotFound() {
        String description = "Nonexistent Category";

        when(categoryRepository.existsByDescription(description)).thenReturn(false);

        CategoryNotFoundException exception = assertThrows(CategoryNotFoundException.class, () -> {
            categoryService.getCategoryByDescription(description);
        });

        assertEquals("Category with description Nonexistent Category does not exist", exception.getMessage());
    }

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
