package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.BadgeNotFoundException;
import pt.upskill.clientapi.Interfaces.BadgeDAO;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.Models.Badge;

import java.util.List;

/**
 * Service class for handling business logic related to badges.
 * This class implements the BadgeDAO interface and provides methods to create, retrieve, and delete badges.
 * It interacts with the BadgeRepository to perform database operations.
 */
@Service
public class BadgeService implements BadgeDAO {
    private final BadgeRepository repository;

    /**
     * Constructs a BadgeService with the provided BadgeRepository.
     *
     * @param repository the BadgeRepository used to interact with the database
     */
    public BadgeService(BadgeRepository repository) {
        this.repository = repository;
    }

    /**
     * Creates a new badge.
     * The method checks if a badge with the same description or interactions already exists.
     * If either exists, an IllegalArgumentException is thrown.
     *
     * @param badge the Badge object to be created
     * @return the created Badge object
     * @throws IllegalArgumentException if a badge with the same description or interactions already exists
     */
    @Override
    public Badge createBadge(Badge badge) {
        if (repository.existsByDescription(badge.getDescription())) {
            throw new IllegalArgumentException(String.format("Badge with description %s already exists", badge.getDescription()));
        }
        if (repository.existsByInteractions(badge.getInteractions())) {
            throw new IllegalArgumentException(String.format("Badge with interactions %s already exists", badge.getInteractions()));
        }
        return repository.save(badge);
    }

    /**
     * Retrieves all badges from the database.
     *
     * @return a list of all Badge objects
     */
    @Override
    public List<Badge> getAllBadges() {
        return repository.findAll();
    }

    /**
     * Deletes a badge by its description.
     * If no badge with the given description exists, a BadgeNotFoundException is thrown.
     *
     * @param description the description of the badge to be deleted
     * @throws BadgeNotFoundException if no badge with the given description exists
     */
    @Override
    public void deleteBadgeByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new BadgeNotFoundException(String.format("Badge with description %s does not exist", description));
        }
        repository.delete(repository.findByDescription(description));
    }
}