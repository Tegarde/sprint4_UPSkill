package pt.upskill.clientapi.Controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.clientapi.CustomExceptions.CategoryNotFoundException;
import pt.upskill.clientapi.DTOs.CategoryDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Interfaces.CategoryDAO;
import pt.upskill.clientapi.Mappers.CategoryMapper;
import pt.upskill.clientapi.Models.Category;

import java.util.List;

@RestController
@RequestMapping("api/category")
public class CategoryController {

    private final CategoryDAO service;

    public CategoryController(CategoryDAO service) {
        this.service = service;
    }

    @PostMapping
    public ResponseEntity<ResponseMessage> createCategory(@RequestBody CategoryDTO dto) {
        try {
            Category category = CategoryMapper.fromDTO(dto);
            Category newCategory = service.createCategory(category);
            return new ResponseEntity<>(new ResponseMessage(String.format("Category %s created successfully", newCategory.getDescription())), HttpStatus.CREATED);
        } catch (IllegalArgumentException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @GetMapping
    public ResponseEntity<?> getAllCategories() {
        try {
            List<CategoryDTO> categories = service.getAllCategories().stream().map(CategoryMapper::toDTO).toList();
            return (categories.isEmpty()) ? new ResponseEntity<>(new ResponseMessage("No categories found"), HttpStatus.NO_CONTENT) : new ResponseEntity<>(categories, HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @GetMapping("{description}")
    public ResponseEntity<?> getCategory(@PathVariable String description) {
        try {
            Category category = service.getCategoryByDescription(description);
            CategoryDTO dto = CategoryMapper.toDTO(category);
            return new ResponseEntity<>(dto, HttpStatus.OK);
        } catch (CategoryNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @DeleteMapping("{description}")
    public ResponseEntity<ResponseMessage> deleteCategory(@PathVariable String description) {
        try {
            service.deleteCategoryByDescription(description);
            return ResponseEntity.noContent().build();
        } catch (CategoryNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }
}
