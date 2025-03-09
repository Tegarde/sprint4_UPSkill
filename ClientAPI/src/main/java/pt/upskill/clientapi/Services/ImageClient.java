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


@Service
public class ImageClient {

    private static final String BASE_URL = "http://localhost:5000/api";

    private final WebClient client;

    public ImageClient() {
        this.client = WebClient.create(BASE_URL);
    }

//    public String saveImage(MultipartFile file) {
//        return client.post()
//                .uri("/fileUpload/upload")
//                .bodyValue(file)
//                .retrieve()
//                .bodyToMono(String.class)
//                .block();
//    }

    public ImageResponseDTO saveImage(MultipartFile file) {
        try {
            // Convert MultipartFile to Resource
            ByteArrayResource fileResource = new ByteArrayResource(file.getBytes()) {
                @Override
                public String getFilename() {
                    return file.getOriginalFilename(); // Set the filename
                }
            };

            // Build the multipart request correctly
            MultipartBodyBuilder bodyBuilder = new MultipartBodyBuilder();
            bodyBuilder.part("file", fileResource)
                    .header("Content-Disposition", "form-data; name=file; filename=" + file.getOriginalFilename())
                    .contentType(MediaType.APPLICATION_OCTET_STREAM);

            return client.post()
                    .uri("/fileUpload/upload")
                    .contentType(MediaType.MULTIPART_FORM_DATA)
                    .bodyValue(bodyBuilder.build()) // Correctly sending multipart data
                    .retrieve()
                    .bodyToMono(ImageResponseDTO.class)
                    .block();
        } catch (Exception e) {
            throw new RuntimeException("Error uploading file: " + e.getMessage(), e);
        }
    }
}
