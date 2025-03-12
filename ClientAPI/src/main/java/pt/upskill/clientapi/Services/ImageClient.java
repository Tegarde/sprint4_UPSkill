package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.reactive.function.client.WebClient;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.http.MediaType;
import org.springframework.http.client.MultipartBodyBuilder;
import pt.upskill.clientapi.DTOs.ImageResponseDTO;
import reactor.core.publisher.Mono;

/**
 * Service class to handle image upload functionality.
 * This class uses a WebClient to interact with an external service to upload images.
 */
@Service
public class ImageClient {

    private static final String BASE_URL = "http://localhost:5000/api"; // URL of the external image service

    private final WebClient client;

    /**
     * Constructs an ImageClient with a WebClient instance.
     */
    public ImageClient() {
        this.client = WebClient.create(BASE_URL);
    }

    /**
     * Saves an image by sending a POST request to an external image upload service.
     * The image is sent as multipart form data, and the response is mapped to an ImageResponseDTO.
     *
     * @param file the image file to be uploaded
     * @return an ImageResponseDTO containing the result of the upload operation
     * @throws RuntimeException if an error occurs during file upload
     */
    public ImageResponseDTO saveImage(MultipartFile file) {
        try {
            // Convert MultipartFile to Resource for WebClient
            ByteArrayResource fileResource = new ByteArrayResource(file.getBytes()) {
                @Override
                public String getFilename() {
                    return file.getOriginalFilename(); // Set the filename for the uploaded file
                }
            };

            // Build the multipart request with the file part
            MultipartBodyBuilder bodyBuilder = new MultipartBodyBuilder();
            bodyBuilder.part("file", fileResource)
                    .header("Content-Disposition", "form-data; name=file; filename=" + file.getOriginalFilename())
                    .contentType(MediaType.APPLICATION_OCTET_STREAM);

            // Send the POST request to upload the file
            return client.post()
                    .uri("/fileUpload/upload")  // URI of the file upload endpoint
                    .contentType(MediaType.MULTIPART_FORM_DATA) // Set the content type to multipart/form-data
                    .bodyValue(bodyBuilder.build()) // Attach the multipart body to the request
                    .retrieve() // Send the request
                    .bodyToMono(ImageResponseDTO.class) // Map the response to ImageResponseDTO
                    .block(); // Block until the response is received
        } catch (Exception e) {
            throw new RuntimeException("Error uploading file: " + e.getMessage(), e);
        }
    }
}