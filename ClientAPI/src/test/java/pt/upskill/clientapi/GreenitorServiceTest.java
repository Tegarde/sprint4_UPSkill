package pt.upskill.clientapi;

import static org.mockito.Mockito.*;
import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import pt.upskill.clientapi.CustomExceptions.UserNotFoundException;
import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.JPARepositories.GreenitorRepository;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;
import pt.upskill.clientapi.Services.GreenitorService;

import java.util.ArrayList;

public class GreenitorServiceTest {

    @Mock
    private GreenitorRepository greenitorRepository;

    @Mock
    private BadgeRepository badgeRepository;

    @InjectMocks
    private GreenitorService greenitorService;

    /**
     * Sets up the test fixture. This is called before each test method.
     */
    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    /**
     * Verifies that a user can be successfully registered.
     * <p>
     * Preconditions: No user with the same email or username exists.
     * Postconditions: The user is registered and returned.
     * <p>
     * Covers: {@link GreenitorService#registerUser(Greenitor)}
     */
    @Test
    void testRegisterUser_Success() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.existsByEmail(greenitor.getEmail())).thenReturn(false);
        when(greenitorRepository.existsByUsername(greenitor.getUsername())).thenReturn(false);
        when(greenitorRepository.save(greenitor)).thenReturn(greenitor);

        Greenitor result = greenitorService.registerUser(greenitor);

        assertNotNull(result);
        assertEquals(greenitor.getUsername(), result.getUsername());
        assertEquals(greenitor.getEmail(), result.getEmail());
    }

    /*
    @Test
    void testRegisterUser_NullValues() {
        Greenitor greenitor = new Greenitor(null, null, null);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.registerUser(greenitor);
        });

        assertEquals("Username, email, and password must not be null", exception.getMessage());
    }
     */

    /**
     * Verifies that attempting to register a user with an existing email fails.
     * <p>
     * Preconditions: A user with the same email exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link GreenitorService#registerUser(Greenitor)}
     */
    @Test
    void testRegisterUser_EmailExists() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.existsByEmail(greenitor.getEmail())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.registerUser(greenitor);
        });

        assertEquals("User with email email@example.com already exists", exception.getMessage());
    }

    /**
     * Verifies that attempting to register a user with an existing username fails.
     * <p>
     * Preconditions: A user with the same username exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link GreenitorService#registerUser(Greenitor)}
     */
    @Test
    void testRegisterUser_UsernameExists() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.existsByUsername(greenitor.getUsername())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.registerUser(greenitor);
        });

        assertEquals("User with username username already exists", exception.getMessage());
    }

    /**
     * Verifies that a user can be successfully logged in.
     * <p>
     * Preconditions: A user with the given email and password exists.
     * Postconditions: A token with the user's username and role is returned.
     * <p>
     * Covers: {@link GreenitorService#loginUser(LoginDTO)}
     */
    @Test
    void testLoginUser_Success() {
        LoginDTO loginDTO = new LoginDTO("email@example.com", "password");
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.findByEmail(loginDTO.getEmail())).thenReturn(greenitor);

        Token result = greenitorService.loginUser(loginDTO);

        assertNotNull(result);
        assertEquals(greenitor.getUsername(), result.getUsername());
        assertEquals(greenitor.getRole(), result.getRole());
    }

    /*
    @Test
    void testLoginUser_NullValues() {
        LoginDTO loginDTO = new LoginDTO(null, null);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.loginUser(loginDTO);
        });

        assertEquals("Email and password must not be null", exception.getMessage());
    }
    */

    /**
     * Verifies that attempting to log in with an email that doesn't exist throws a UserNotFoundException.
     * <p>
     * Preconditions: No user with the given email exists.
     * Postconditions: A UserNotFoundException is thrown.
     * <p>
     * Covers: {@link GreenitorService#loginUser(LoginDTO)}
     */
    @Test
    void testLoginUser_UserNotFound() {
        LoginDTO loginDTO = new LoginDTO("email@example.com", "password");

        when(greenitorRepository.findByEmail(loginDTO.getEmail())).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.loginUser(loginDTO);
        });

        assertEquals("User with email email@example.com does not exist", exception.getMessage());
    }

    /**
     * Verifies that attempting to log in with an invalid password throws an
     * IllegalArgumentException.
     * <p>
     * Preconditions: A user with the given email exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link GreenitorService#loginUser(LoginDTO)}
     */
    @Test
    void testLoginUser_InvalidPassword() {
        LoginDTO loginDTO = new LoginDTO("email@example.com", "password");
        Greenitor greenitor = new Greenitor("username", "email@example.com", "wrongpassword");

        when(greenitorRepository.findByEmail(loginDTO.getEmail())).thenReturn(greenitor);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.loginUser(loginDTO);
        });

        assertEquals("Invalid password", exception.getMessage());
    }

    /**
     * Verifies that a user can be successfully retrieved by its username.
     * <p>
     * Preconditions: A user with the given username exists.
     * Postconditions: The user is returned.
     * <p>
     * Covers: {@link GreenitorService#getGreenitorByUsername(String)}
     */
    @Test
    void testGetGreenitorByUsername_Success() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.findByUsername("username")).thenReturn(greenitor);

        Greenitor result = greenitorService.getGreenitorByUsername("username");

        assertNotNull(result);
        assertEquals(greenitor.getUsername(), result.getUsername());
        assertEquals(greenitor.getEmail(), result.getEmail());
    }


    /*@Test
    void testGetGreenitorByUsername_NullValue() {
        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.getGreenitorByUsername(null);
        });

        assertEquals("Username must not be null", exception.getMessage());
    }
    */

    /**
     * Verifies that attempting to retrieve a user with a non-existent username throws a UserNotFoundException.
     * <p>
     * Preconditions: No user with the given username exists.
     * Postconditions: A UserNotFoundException is thrown.
     * <p>
     * Covers: {@link GreenitorService#getGreenitorByUsername(String)}
     */
    @Test
    void testGetGreenitorByUsername_UserNotFound() {
        when(greenitorRepository.findByUsername("username")).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.getGreenitorByUsername("username");
        });

        assertEquals("User with username username does not exist", exception.getMessage());
    }

    /**
     * Verifies that attempting to increment the interactions of a user that doesn't exist throws a UserNotFoundException.
     * <p>
     * Preconditions: No user with the given username exists.
     * Postconditions: A UserNotFoundException is thrown.
     * <p>
     * Covers: {@link GreenitorService#incrementInteractions(String)}
     */
    @Test
    void testIncrementInteractions_UserNotFound() {
        when(greenitorRepository.findByUsername("nonexistent")).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.incrementInteractions("nonexistent");
        });

        assertEquals("User with username nonexistent does not exist", exception.getMessage());
    }

    /*
    @Test
    void testIncrementInteractions_NullUsername() {
        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.incrementInteractions(null);
        });

        assertEquals("Username must not be null", exception.getMessage());
    }

    @Test
    void testIncrementInteractions_MaxInteractions() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");
        greenitor.setInteractions(Integer.MAX_VALUE);

        when(greenitorRepository.findByUsername("username")).thenReturn(greenitor);
        when(badgeRepository.findBadgesByGreenitorId(greenitor.getId())).thenReturn(new ArrayList<>());

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.incrementInteractions("username");
        });

        assertEquals("Maximum interactions reached", exception.getMessage());
    }
    */

    /**
     * Verifies that attempting to decrement the interactions of a user that doesn't exist throws a UserNotFoundException.
     * <p>
     * Preconditions: No user with the given username exists.
     * Postconditions: A UserNotFoundException is thrown.
     * <p>
     * Covers: {@link GreenitorService#decrementInteractions(String)}
     */
    @Test
    void testDecrementInteractions_UserNotFound() {
        when(greenitorRepository.findByUsername("nonexistent")).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.decrementInteractions("nonexistent");
        });

        assertEquals("User with username nonexistent does not exist", exception.getMessage());
    }

    /*
    @Test
    void testDecrementInteractions_NullUsername() {
        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.decrementInteractions(null);
        });

        assertEquals("Username must not be null", exception.getMessage());
    }

    @Test
    void testDecrementInteractions_ZeroInteractions() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");
        greenitor.setInteractions(0);

        when(greenitorRepository.findByUsername("username")).thenReturn(greenitor);
        when(badgeRepository.findBadgesByGreenitorId(greenitor.getId())).thenReturn(new ArrayList<>());

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.decrementInteractions("username");
        });

        assertEquals("No interactions to decrement", exception.getMessage());
    }
     */
}
