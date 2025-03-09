package pt.upskill.clientapi.Controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import pt.upskill.clientapi.CustomExceptions.BadgeNotFoundException;
import pt.upskill.clientapi.DTOs.BadgeDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Interfaces.BadgeDAO;
import pt.upskill.clientapi.Mappers.BadgeMapper;
import pt.upskill.clientapi.Models.Badge;
import pt.upskill.clientapi.Services.ImageClient;

@RestController
@RequestMapping("api/badges")
public class BadgeController {

    private final BadgeDAO service;
    @Autowired
    private final ImageClient imageClient;

    public BadgeController(BadgeDAO service, ImageClient imageClient) {
        this.service = service;
        this.imageClient = imageClient;
    }
    @PostMapping(consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<ResponseMessage> createBadge(
            @RequestPart("description") String description,
            @RequestPart("interactions") String interactions,  // Changed to Integer to handle null cases
            @RequestPart("image") MultipartFile image) {

        try {
            if (image.isEmpty()) {
                return ResponseEntity.badRequest().body(new ResponseMessage("No file uploaded"));
            }

            // Convert String to int safely
            int interactionsN = 0; // Default value
            if (interactions != null && !interactions.isEmpty()) {
                try {
                    interactionsN = Integer.parseInt(interactions);
                } catch (NumberFormatException e) {
                    return ResponseEntity.badRequest().body(new ResponseMessage("Invalid number format for interactions"));
                }
            }

            // Create DTO
            BadgeDTO dto = new BadgeDTO(description, interactionsN);

            // Save the image and get the URL
            String url = imageClient.saveImage(image).fileName;

            // Convert DTO to entity and set image URL
            Badge newBadge = BadgeMapper.fromDTO(dto);
            newBadge.setImage(url);
            service.createBadge(newBadge);

            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(new ResponseMessage(String.format("Badge %s created successfully", dto.getDescription())));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(new ResponseMessage(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(new ResponseMessage("Something went wrong"));
        }
    }

    @GetMapping
    public ResponseEntity<?> getAllBadges() {
        try {
            return new ResponseEntity<>(service.getAllBadges().stream().map(BadgeMapper::toDTO).toList(), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @DeleteMapping("{description}")
    public ResponseEntity<ResponseMessage> deleteBadge(@PathVariable String description) {
        try {
            service.deleteBadgeByDescription(description);
            return ResponseEntity.noContent().build();
        } catch (BadgeNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }
}
