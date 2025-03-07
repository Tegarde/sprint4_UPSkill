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

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

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

    @Test
    void testRegisterUser_EmailExists() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.existsByEmail(greenitor.getEmail())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.registerUser(greenitor);
        });

        assertEquals("User with email email@example.com already exists", exception.getMessage());
    }

    @Test
    void testRegisterUser_UsernameExists() {
        Greenitor greenitor = new Greenitor("username", "email@example.com", "password");

        when(greenitorRepository.existsByUsername(greenitor.getUsername())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            greenitorService.registerUser(greenitor);
        });

        assertEquals("User with username username already exists", exception.getMessage());
    }

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

    @Test
    void testLoginUser_UserNotFound() {
        LoginDTO loginDTO = new LoginDTO("email@example.com", "password");

        when(greenitorRepository.findByEmail(loginDTO.getEmail())).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.loginUser(loginDTO);
        });

        assertEquals("User with email email@example.com does not exist", exception.getMessage());
    }

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

    @Test
    void testGetGreenitorByUsername_UserNotFound() {
        when(greenitorRepository.findByUsername("username")).thenReturn(null);

        UserNotFoundException exception = assertThrows(UserNotFoundException.class, () -> {
            greenitorService.getGreenitorByUsername("username");
        });

        assertEquals("User with username username does not exist", exception.getMessage());
    }

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
