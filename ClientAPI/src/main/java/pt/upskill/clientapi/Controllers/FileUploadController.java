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

@Tag(name = "File Upload Management")
@RestController
@RequestMapping("api/file")
public class FileUploadController {

    @Autowired
    private final ImageClient imageClient;

    public FileUploadController(ImageClient imageClient) {
        this.imageClient = imageClient;
    }

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