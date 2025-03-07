package pt.upskill.clientapi;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import pt.upskill.clientapi.CustomExceptions.BadgeNotFoundException;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.Models.Badge;
import pt.upskill.clientapi.Services.BadgeService;

import java.util.Collections;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class BadgeServiceTest {

    @Mock
    private BadgeRepository badgeRepository;

    @InjectMocks
    private BadgeService badgeService;

    /**
     * Set up the test fixture. This is called before each test method.
     */
    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    /**
     * Verifies that a badge can be successfully created.
     * <p>
     * Preconditions: No badge with the same description or interactions exists.
     * Postconditions: The badge is created and returned.
     * <p>
     * Covers: {@link BadgeService#createBadge(Badge)}
     */
    @Test
    void testCreateBadge_Success() {
        Badge badge = new Badge("New Badge", 10);

        when(badgeRepository.existsByDescription(badge.getDescription())).thenReturn(false);
        when(badgeRepository.existsByInteractions(badge.getInteractions())).thenReturn(false);
        when(badgeRepository.save(badge)).thenReturn(badge);

        Badge result = badgeService.createBadge(badge);

        assertNotNull(result);
        assertEquals(badge.getDescription(), result.getDescription());
        assertEquals(badge.getInteractions(), result.getInteractions());
    }

    /**
     * Verifies that creating a badge with an existing description fails.
     * <p>
     * Preconditions: A badge with the same description exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link BadgeService#createBadge(Badge)}
     */
    @Test
    void testCreateBadge_DescriptionExists() {
        Badge badge = new Badge("Existing Badge", 10);

        when(badgeRepository.existsByDescription(badge.getDescription())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            badgeService.createBadge(badge);
        });

        assertEquals("Badge with description Existing Badge already exists", exception.getMessage());
    }

    /**
     * Verifies that creating a badge with existing interactions fails.
     * <p>
     * Preconditions: A badge with the same interactions exists.
     * Postconditions: An IllegalArgumentException is thrown.
     * <p>
     * Covers: {@link BadgeService#createBadge(Badge)}
     */
    @Test
    void testCreateBadge_InteractionsExists() {
        Badge badge = new Badge("New Badge", 10);

        when(badgeRepository.existsByInteractions(badge.getInteractions())).thenReturn(true);

        IllegalArgumentException exception = assertThrows(IllegalArgumentException.class, () -> {
            badgeService.createBadge(badge);
        });

        assertEquals("Badge with interactions 10 already exists", exception.getMessage());
    }

    /**
     * Verifies that all badges can be successfully retrieved.
     * <p>
     * Preconditions: A single badge exists in the repository.
     * Postconditions: A list containing the existing badge is returned.
     * <p>
     * Covers: {@link BadgeService#getAllBadges()}
     */
    @Test
    void testGetAllBadges() {
        Badge badge = new Badge("Badge", 10);

        when(badgeRepository.findAll()).thenReturn(Collections.singletonList(badge));

        List<Badge> result = badgeService.getAllBadges();

        assertNotNull(result);
        assertEquals(1, result.size());
        assertEquals(badge.getDescription(), result.get(0).getDescription());
    }

    /**
     * Verifies that a badge can be successfully deleted by its description.
     * <p>
     * Preconditions: A badge with the specified description exists in the repository.
     * Postconditions: The badge with the specified description is deleted.
     * <p>
     * Covers: {@link BadgeService#deleteBadgeByDescription(String)}
     */
    @Test
    void testDeleteBadgeByDescription_Success() {
        Badge badge = new Badge("Badge to Delete", 10);

        when(badgeRepository.existsByDescription(badge.getDescription())).thenReturn(true);
        when(badgeRepository.findByDescription(badge.getDescription())).thenReturn(badge);

        assertDoesNotThrow(() -> badgeService.deleteBadgeByDescription(badge.getDescription()));

        verify(badgeRepository, times(1)).delete(badge);
    }

    /**
     * Verifies that attempting to delete a badge by its description fails when the badge does not exist.
     * <p>
     * Preconditions: No badge with the specified description exists in the repository.
     * Postconditions: A BadgeNotFoundException is thrown.
     * <p>
     * Covers: {@link BadgeService#deleteBadgeByDescription(String)}
     */
    @Test
    void testDeleteBadgeByDescription_NotFound() {
        String description = "Nonexistent Badge";

        when(badgeRepository.existsByDescription(description)).thenReturn(false);

        BadgeNotFoundException exception = assertThrows(BadgeNotFoundException.class, () -> {
            badgeService.deleteBadgeByDescription(description);
        });

        assertEquals("Badge with description Nonexistent Badge does not exist", exception.getMessage());
    }
}
