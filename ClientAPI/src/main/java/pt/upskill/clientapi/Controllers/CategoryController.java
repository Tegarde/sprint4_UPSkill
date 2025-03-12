package pt.upskill.clientapi.Controllers;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
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

@Tag(name = "Category Management", description = "Operations related to categories")
@RestController
@RequestMapping("api/category")
public class CategoryController {

    private final CategoryDAO service;

    public CategoryController(CategoryDAO service) {
        this.service = service;
    }

    @Operation(summary = "Create a new Category", description = "Creates a new category with a description.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "201", description = "Category created successfully",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Invalid input",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
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

    @Operation(summary = "Get all Categories", description = "Fetches a list of all available categories.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "List of categories retrieved successfully",
                    content = @Content),
            @ApiResponse(responseCode = "204", description = "No categories found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @GetMapping
    public ResponseEntity<?> getAllCategories() {
        try {
            List<CategoryDTO> categories = service.getAllCategories().stream().map(CategoryMapper::toDTO).toList();
            return (categories.isEmpty()) ? new ResponseEntity<>(new ResponseMessage("No categories found"), HttpStatus.NO_CONTENT) : new ResponseEntity<>(categories, HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @Operation(summary = "Get a Category by Description", description = "Fetches a category by its description.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Category retrieved successfully",
                    content = @Content(schema = @Schema(implementation = CategoryDTO.class))),
            @ApiResponse(responseCode = "404", description = "Category not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
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

    @Operation(summary = "Delete a Category", description = "Deletes a category by its description.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "204", description = "Category deleted successfully"),
            @ApiResponse(responseCode = "404", description = "Category not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
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