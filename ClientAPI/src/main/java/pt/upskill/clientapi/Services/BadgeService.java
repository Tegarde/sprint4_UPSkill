package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.BadgeNotFoundException;
import pt.upskill.clientapi.Interfaces.BadgeDAO;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.Models.Badge;

import java.util.List;

@Service
public class BadgeService implements BadgeDAO {
    private final BadgeRepository repository;

    public BadgeService(BadgeRepository repository) {
        this.repository = repository;
    }

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

    @Override
    public List<Badge> getAllBadges() {
        return repository.findAll();
    }

    @Override
    public void deleteBadgeByDescription(String description) {
        if (!repository.existsByDescription(description)) {
            throw new BadgeNotFoundException(String.format("Badge with description %s does not exist", description));
        }
        repository.delete(repository.findByDescription(description));
    }
}
