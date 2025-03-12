package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object (DTO) for a Category.
 * This class represents the data associated with a category, including its description.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CategoryDTO {

    /**
     * Description of the category.
     * This field provides a textual representation of what the category signifies or its purpose.
     */
    private String description;
}