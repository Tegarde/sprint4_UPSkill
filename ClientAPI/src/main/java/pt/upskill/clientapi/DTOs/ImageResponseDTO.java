package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Data Transfer Object (DTO) for image response.
 * This class is used to represent the response returned after an image upload.
 * It contains the message regarding the result of the upload operation and the name of the uploaded file.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class ImageResponseDTO {

    /**
     * Message indicating the result of the image upload.
     * This field provides feedback, such as success or error, after an image upload operation.
     */
    public String message;

    /**
     * The name of the uploaded file.
     * This field holds the name of the image file that was uploaded successfully.
     */
    public String fileName;
}