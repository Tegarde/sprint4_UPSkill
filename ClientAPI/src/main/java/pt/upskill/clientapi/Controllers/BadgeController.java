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

/**
 * Controller responsible for managing user badges. This includes creating, retrieving,
 * and deleting badges, as well as handling badge-related operations like managing interactions
 * and image uploads.
 */
@Tag(name = "Badge Management", description = "Operations related to User Badges")
@RestController
@RequestMapping("api/badges")
public class BadgeController {

    private final BadgeDAO service;
    private final ImageClient imageClient;

    /**
     * Constructor to initialize BadgeController.
     *
     * @param service The BadgeDAO service used to interact with the badge database.
     * @param imageClient The ImageClient used to handle image uploads.
     */
    @Autowired
    public BadgeController(BadgeDAO service, ImageClient imageClient) {
        this.service = service;
        this.imageClient = imageClient;
    }

    /**
     * Endpoint to create a new badge. The badge includes a description, interaction count,
     * and an associated image.
     *
     * @param description The description of the badge.
     * @param interactions The interaction count required for the badge.
     * @param image The image associated with the badge.
     * @return A ResponseEntity containing a message and HTTP status indicating success or failure.
     */
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

    /**
     * Endpoint to retrieve all available badges.
     *
     * @return A ResponseEntity containing a list of all badges or an error message if something went wrong.
     */
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

    /**
     * Endpoint to delete a badge by its description.
     *
     * @param description The description of the badge to be deleted.
     * @return A ResponseEntity containing an HTTP status indicating success or failure.
     */
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