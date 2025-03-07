package pt.upskill.clientapi.Services;

import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.UserNotFoundException;
import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.JPARepositories.GreenitorRepository;
import pt.upskill.clientapi.Models.Badge;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

import java.util.Comparator;
import java.util.List;

@Service
public class GreenitorService implements GreenitorDAO {

    private final GreenitorRepository greenitorRepository;

    private final BadgeRepository badgeRepository;

    public GreenitorService(GreenitorRepository greenitorRepository, BadgeRepository badgeRepository) {
        this.badgeRepository = badgeRepository;
        this.greenitorRepository = greenitorRepository;
    }


    @Override
    public Greenitor registerUser(Greenitor greenitor) {
        if (greenitorRepository.existsByEmail(greenitor.getEmail())) {
            throw new IllegalArgumentException(String.format("User with email %s already exists", greenitor.getEmail()));
        }

        if (greenitorRepository.existsByUsername(greenitor.getUsername())) {
            throw new IllegalArgumentException(String.format("User with username %s already exists", greenitor.getUsername()));
        }
        return greenitorRepository.save(greenitor);
    }

    @Override

    public Token loginUser(LoginDTO loginDTO) {
        Greenitor greenitor = greenitorRepository.findByEmail(loginDTO.getEmail());

        if (greenitor == null) {
            throw new UserNotFoundException(String.format("User with email %s does not exist", loginDTO.getEmail()));
        }
        if (!greenitor.getPassword().equals(loginDTO.getPassword())) {
            throw new IllegalArgumentException("Invalid password");
        }
        return new Token(greenitor.getUsername(), greenitor.getRole());
    }

    @Override
    public Greenitor getGreenitorByUsername(String username) {
        Greenitor greenitor = greenitorRepository.findByUsername(username);
        if (greenitor == null) {
            throw new UserNotFoundException(String.format("User with username %s does not exist", username));
        }
        greenitor.setBadges(badgeRepository.findBadgesByGreenitorId(greenitor.getId()));

        return greenitor;
    }

    @Override
    @Transactional
    public ResponseMessage incrementInteractions(String username) {
        Greenitor greenitor = getGreenitorByUsername(username);
        greenitor.setBadges(badgeRepository.findBadgesByGreenitorId(greenitor.getId()));
        greenitor.setInteractions(greenitor.getInteractions() + 1);

        List<Badge> badges = badgeRepository.findAllSortedByInteractions();

        for (Badge badge : badges) {
            if (greenitor.getInteractions() == badge.getInteractions()) {
                greenitor.getBadges().add(badge);
                greenitorRepository.save(greenitor);
                return new ResponseMessage(String.format("Badge %s unlocked", badge.getDescription()));
            }
        }
        greenitorRepository.save(greenitor);
        return new ResponseMessage("Interactions incremented");
    }

    @Override
    @Transactional
    public ResponseMessage decrementInteractions(String username) {
        Greenitor greenitor = getGreenitorByUsername(username);
        greenitor.setBadges(badgeRepository.findBadgesByGreenitorId(greenitor.getId()));
        List<Badge> badges = badgeRepository.findAllSortedByInteractions();

        for (Badge badge : badges) {
            if (greenitor.getInteractions() == badge.getInteractions()) {
                greenitor.getBadges().remove(badge);
                greenitor.setInteractions(greenitor.getInteractions() - 1);
                greenitorRepository.save(greenitor);
                return new ResponseMessage(String.format("Badge %s removed", badge.getDescription()));
            }
        }
        greenitor.setInteractions(greenitor.getInteractions() - 1);
        greenitorRepository.save(greenitor);
        return new ResponseMessage("Interactions decremented");
    }
}
