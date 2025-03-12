package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Badge;

import java.util.List;

/**
 * Repository interface for managing Badge entities.
 * Provides methods for querying the Badge table in the database.
 */
@Repository
public interface BadgeRepository extends JpaRepository<Badge, Integer> {

    /**
     * Retrieves a list of all badges, sorted by interactions.
     *
     * @return A list of Badge objects ordered by interactions.
     */
    @Query("SELECT b FROM Badge b ORDER BY b.interactions")
    List<Badge> findAllSortedByInteractions();

    /**
     * Retrieves a list of badges associated with a specific Greenitor by their ID.
     *
     * @param greenitorId The ID of the Greenitor whose badges are to be retrieved.
     * @return A list of Badge objects associated with the specified Greenitor.
     */
    @Query("SELECT b FROM Greenitor g JOIN g.badges b WHERE g.id = :greenitorId")
    List<Badge> findBadgesByGreenitorId(@Param("greenitorId") int greenitorId);

    /**
     * Checks if a badge with the specified description exists.
     *
     * @param description The description of the badge to check.
     * @return true if a badge with the given description exists, otherwise false.
     */
    boolean existsByDescription(String description);

    /**
     * Checks if a badge with the specified number of interactions exists.
     *
     * @param interactions The number of interactions to check.
     * @return true if a badge with the given number of interactions exists, otherwise false.
     */
    boolean existsByInteractions(int interactions);

    /**
     * Retrieves a badge by its description.
     *
     * @param description The description of the badge to retrieve.
     * @return The Badge object corresponding to the given description.
     */
    Badge findByDescription(String description);
}