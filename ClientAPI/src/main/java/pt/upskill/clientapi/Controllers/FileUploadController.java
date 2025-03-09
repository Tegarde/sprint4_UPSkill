package pt.upskill.clientapi.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestPart;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;
import pt.upskill.clientapi.DTOs.ImageResponseDTO;
import pt.upskill.clientapi.Services.ImageClient;

@RestController
@RequestMapping("api/file")
public class FileUploadController {

    @Autowired
    private final ImageClient imageClient;

    public FileUploadController(ImageClient imageClient) {
        this.imageClient = imageClient;
    }

    @PostMapping("/upload")
    public ResponseEntity<String> uploadFile(@RequestPart("file") MultipartFile file) {
        if (file.isEmpty()) {
            return ResponseEntity.badRequest().body("No file uploaded");
        }
        String url = imageClient.saveImage(file).fileName;

        return ResponseEntity.ok("File uploaded: " + url);
    }
}
