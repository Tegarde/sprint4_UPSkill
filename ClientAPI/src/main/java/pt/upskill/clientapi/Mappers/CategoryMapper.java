package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.CategoryDTO;
import pt.upskill.clientapi.Models.Category;

/**
 * Mapper class for converting between Category entities and Category Data Transfer Objects (DTOs).
 * Provides methods for converting from and to DTO representations.
 */
public class CategoryMapper {

    /**
     * Converts a CategoryDTO to a Category entity.
     *
     * @param dto The CategoryDTO to convert.
     * @return A Category entity populated with data from the provided DTO.
     */
    public static Category fromDTO(CategoryDTO dto) {
        return new Category(dto.getDescription());
    }

    /**
     * Converts a Category entity to a CategoryDTO.
     *
     * @param category The Category entity to convert.
     * @return A CategoryDTO populated with data from the provided Category entity.
     */
    public static CategoryDTO toDTO(Category category) {
        return new CategoryDTO(category.getDescription());
    }
}