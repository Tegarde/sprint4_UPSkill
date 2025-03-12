package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.Models.Badge;

import java.util.List;

/**
 * Interface for Badge Data Access Object (DAO).
 * This interface defines the operations for managing badges in the system.
 */
public interface BadgeDAO {

    /**
     * Creates a new badge in the system.
     *
     * @param badge The badge to be created.
     * @return The created badge with any generated properties (e.g., ID).
     */
    Badge createBadge(Badge badge);

    /**
     * Retrieves a list of all badges in the system.
     *
     * @return A list of all badges.
     */
    List<Badge> getAllBadges();

    /**
     * Deletes a badge based on its description.
     *
     * @param description The description of the badge to be deleted.
     */
    void deleteBadgeByDescription(String description);
}