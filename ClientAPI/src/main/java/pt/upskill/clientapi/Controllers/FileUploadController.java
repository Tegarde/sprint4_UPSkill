package pt.upskill.clientapi.Controllers;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestPart;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;
import pt.upskill.clientapi.Services.ImageClient;

/**
 * Controller responsible for handling file uploads. This includes uploading a file
 * and returning the URL of the stored file.
 */
@Tag(name = "File Upload Management")
@RestController
@RequestMapping("api/file")
public class FileUploadController {

    @Autowired
    private final ImageClient imageClient;

    /**
     * Constructor to initialize the FileUploadController with an ImageClient.
     *
     * @param imageClient The service responsible for storing files and returning URLs.
     */
    @Autowired
    public FileUploadController(ImageClient imageClient) {
        this.imageClient = imageClient;
    }

    /**
     * Endpoint to upload a file. The file is processed, and its URL is returned.
     *
     * @param file The file to be uploaded, passed as a MultipartFile in the request.
     * @return A ResponseEntity containing the URL of the uploaded file or an error message if no file is uploaded.
     */
    @Operation(summary = "Upload a file", description = "Uploads a file and returns the URL of the stored file.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "File uploaded successfully",
                    content = @Content(schema = @Schema(implementation = String.class))),
            @ApiResponse(responseCode = "400", description = "No file uploaded",
                    content = @Content(schema = @Schema(implementation = String.class)))
    })
    @PostMapping("/upload")
    public ResponseEntity<String> uploadFile(@RequestPart("file") MultipartFile file) {
        if (file.isEmpty()) {
            return ResponseEntity.badRequest().body("No file uploaded");
        }
        String url = imageClient.saveImage(file).fileName;
        return ResponseEntity.ok("File uploaded: " + url);
    }
}