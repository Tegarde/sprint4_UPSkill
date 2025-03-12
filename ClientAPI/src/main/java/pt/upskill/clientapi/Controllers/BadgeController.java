package pt.upskill.clientapi.Controllers;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
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

@Tag(name = "Badge Management", description = "Operations related to User Badges")
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

    @Operation(summary = "Create a new Badge", description = "Creates a new badge with a description, interactions, and an image.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "201", description = "Badge created successfully",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Invalid input or missing data",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @PostMapping(consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<ResponseMessage> createBadge(
            @RequestPart("description") String description,
            @RequestPart("interactions") String interactions,
            @RequestPart("image") MultipartFile image) {
        try {
            if (image.isEmpty()) {
                return ResponseEntity.badRequest().body(new ResponseMessage("No file uploaded"));
            }

            int interactionsN = 0;
            if (interactions != null && !interactions.isEmpty()) {
                try {
                    interactionsN = Integer.parseInt(interactions);
                } catch (NumberFormatException e) {
                    return ResponseEntity.badRequest().body(new ResponseMessage("Invalid number format for interactions"));
                }
            }

            BadgeDTO dto = new BadgeDTO(description, interactionsN);
            String url = imageClient.saveImage(image).fileName;
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

    @Operation(summary = "Get all Badges", description = "Fetches a list of all available badges.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "List of badges retrieved successfully",
                    content = @Content),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @GetMapping
    public ResponseEntity<?> getAllBadges() {
        try {
            return new ResponseEntity<>(service.getAllBadges().stream().map(BadgeMapper::toDTO).toList(), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @Operation(summary = "Delete a Badge", description = "Deletes a badge by its description.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "204", description = "Badge deleted successfully"),
            @ApiResponse(responseCode = "404", description = "Badge not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
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