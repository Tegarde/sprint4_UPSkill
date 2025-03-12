package pt.upskill.clientapi.Controllers;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.clientapi.CustomExceptions.UserNotFoundException;
import pt.upskill.clientapi.DTOs.*;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.Mappers.GreenitorMapper;
import pt.upskill.clientapi.Models.Token;

/**
 * Controller class to manage Greenitor operations.
 * This class handles HTTP requests related to Greenitor operations such as
 * registering a new user, logging in, incrementing and decrementing interactions,
 * and retrieving user data.
 */
@Tag(name = "Greenitor Management", description = "Operations related to Greenitors")
@RestController
@RequestMapping("api/greenitor")
public class GreenitorController {

    private final GreenitorDAO service;

    /**
     * Constructor for GreenitorController.
     *
     * @param service the GreenitorDAO service to handle business logic
     */
    public GreenitorController(GreenitorDAO service) {
        this.service = service;
    }

    /**
     * Registers a new Greenitor (user).
     *
     * @param greenitor the data of the Greenitor to be registered
     * @return ResponseEntity with the status of the registration
     */
    @Operation(summary = "Register a new Greenitor", description = "Registers a new user and returns a success message.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "201", description = "User registered successfully",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Invalid input",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @PostMapping
    public ResponseEntity<ResponseMessage> registerUser(RegisterUserDTO greenitor) {
        try {
            service.registerUser(GreenitorMapper.fromRegisterDTO(greenitor));
            return new ResponseEntity<>(new ResponseMessage("User registered successfully"), HttpStatus.CREATED);
        } catch (IllegalArgumentException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    /**
     * Logs in a Greenitor with the provided credentials.
     *
     * @param loginDTO the login credentials for the Greenitor
     * @return ResponseEntity with the login token or error message
     */
    @Operation(summary = "Login a Greenitor", description = "Authenticates a user based on provided credentials and returns a login token.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "User logged in successfully",
                    content = @Content),
            @ApiResponse(responseCode = "401", description = "Unauthorized - Invalid credentials",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "404", description = "User not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @PostMapping("/login")
    public ResponseEntity<?> loginUser(@RequestBody LoginDTO loginDTO) {
        try {
            return new ResponseEntity<>(service.loginUser(loginDTO), HttpStatus.OK);
        } catch (UserNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        }catch (IllegalArgumentException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.UNAUTHORIZED);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    /**
     * Retrieves Greenitor data based on the username.
     *
     * @param username the username of the Greenitor
     * @return ResponseEntity with the Greenitor data
     */
    @Operation(summary = "Get Greenitor by Username", description = "Fetches user data based on the username.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "User data retrieved successfully",
                    content = @Content(schema = @Schema(implementation = GreenitorDTO.class))),
            @ApiResponse(responseCode = "404", description = "User not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @GetMapping("/user/{username}")
    public ResponseEntity<?> getGreenitorByUsername(@PathVariable String username) {
        try {
            GreenitorDTO dto = GreenitorMapper.fromGreenitor(service.getGreenitorByUsername(username));
            return new ResponseEntity<>(dto, HttpStatus.OK);
        } catch (UserNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    /**
     * Increments the interactions of a Greenitor by their username.
     *
     * @param username the username of the Greenitor whose interactions will be incremented
     * @return ResponseEntity with a success message
     */
    @Operation(summary = "Increment Greenitor Interactions", description = "Increments the interactions for a given Greenitor by their username.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Interactions incremented successfully",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "404", description = "User not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @PatchMapping("increment/{username}")
    public ResponseEntity<ResponseMessage> incrementInteractions(@PathVariable String username) {
        try {
            return new ResponseEntity<>(service.incrementInteractions(username), HttpStatus.OK);
        } catch (UserNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    /**
     * Decrements the interactions of a Greenitor by their username.
     *
     * @param username the username of the Greenitor whose interactions will be decremented
     * @return ResponseEntity with a success message
     */
    @Operation(summary = "Decrement Greenitor Interactions", description = "Decrements the interactions for a given Greenitor by their username.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Interactions decremented successfully",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "404", description = "User not found",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @PatchMapping("decrement/{username}")
    public ResponseEntity<ResponseMessage> decrementInteractions(@PathVariable String username) {
        try {
            return new ResponseEntity<>(service.decrementInteractions(username), HttpStatus.OK);
        } catch (UserNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    /**
     * Retrieves a list of all Greenitors.
     *
     * @return ResponseEntity with a list of all Greenitors
     */
    @Operation(summary = "Get All Greenitors", description = "Retrieves a list of all Greenitors.")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "List of Greenitors retrieved successfully",
                    content = @Content(schema = @Schema(implementation = GreenitorDTO.class))),
            @ApiResponse(responseCode = "400", description = "Something went wrong",
                    content = @Content(schema = @Schema(implementation = ResponseMessage.class)))
    })
    @GetMapping("")
    public ResponseEntity<?> getAllGreenitors() {
        try {
            return new ResponseEntity<>(service.getAllGreenitors().stream().map(GreenitorMapper::toGreenitorSearchDTO).toList(), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @PutMapping("/{username}")
    public ResponseEntity<ResponseMessage> updateGreenitor(@PathVariable String username, @RequestBody UpdateGreenitorDTO greenitor) {
        try {
            return new ResponseEntity<>(service.updateGreenitor(username, greenitor), HttpStatus.OK);
        } catch (UserNotFoundException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.NOT_FOUND);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }

    }}

