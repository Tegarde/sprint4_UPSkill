package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.CategoryDTO;
import pt.upskill.clientapi.Models.Category;

/**
 * Mapper class for the Category entity
 */
public class CategoryMapper {
    public static Category fromDTO(CategoryDTO dto) {
        return new Category(dto.getDescription());
    }

    public static CategoryDTO toDTO(Category category) {
        return new CategoryDTO(category.getDescription());
    }
}
