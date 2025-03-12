package pt.upskill.clientapi.Services;

import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.UserNotFoundException;
import pt.upskill.clientapi.DTOs.GreenitorDTO;
import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.DTOs.UpdateGreenitorDTO;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.BadgeRepository;
import pt.upskill.clientapi.JPARepositories.GreenitorRepository;
import pt.upskill.clientapi.Models.Badge;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

import java.util.Comparator;
import java.util.List;

/**
 * Service class for handling business logic related to greenitors (users).
 * This class implements the GreenitorDAO interface and provides methods for user registration, login,
 * managing interactions, and retrieving user details.
 */
@Service
public class GreenitorService implements GreenitorDAO {

    private final GreenitorRepository greenitorRepository;

    private final BadgeRepository badgeRepository;

    /**
     * Constructs a GreenitorService with the provided GreenitorRepository and BadgeRepository.
     *
     * @param greenitorRepository the GreenitorRepository used to interact with the Greenitor data
     * @param badgeRepository the BadgeRepository used to interact with the Badge data
     */
    public GreenitorService(GreenitorRepository greenitorRepository, BadgeRepository badgeRepository) {
        this.badgeRepository = badgeRepository;
        this.greenitorRepository = greenitorRepository;
    }

    /**
     * Registers a new user.
     * Checks if the email or username already exists in the repository.
     * If they exist, an IllegalArgumentException is thrown.
     *
     * @param greenitor the Greenitor object to be registered
     * @return the registered Greenitor object
     * @throws IllegalArgumentException if the email or username already exists
     */
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

    /**
     * Logs in a user.
     * The method validates the email and password and generates a token if valid.
     * If the email is not found or the password is incorrect, exceptions are thrown.
     *
     * @param loginDTO the LoginDTO containing the user's login credentials (email and password)
     * @return a Token object containing the username and role of the logged-in user
     * @throws UserNotFoundException if the user with the provided email does not exist
     * @throws IllegalArgumentException if the provided password is incorrect
     */
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

    /**
     * Retrieves a Greenitor object by their username.
     * If the user does not exist, a UserNotFoundException is thrown.
     *
     * @param username the username of the Greenitor to be retrieved
     * @return the Greenitor object corresponding to the username
     * @throws UserNotFoundException if the user with the provided username does not exist
     */
    @Override
    public Greenitor getGreenitorByUsername(String username) {
        Greenitor greenitor = greenitorRepository.findByUsername(username);
        if (greenitor == null) {
            throw new UserNotFoundException(String.format("User with username %s does not exist", username));
        }
        greenitor.setBadges(badgeRepository.findBadgesByGreenitorId(greenitor.getId()));

        return greenitor;
    }

    /**
     * Increments the interactions count for a Greenitor.
     * It checks if the increment leads to unlocking any new badges based on interactions.
     * If a badge is unlocked, it is added to the user's badges.
     *
     * @param username the username of the Greenitor whose interactions are to be incremented
     * @return a ResponseMessage indicating the result of the interaction increment
     */
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

    /**
     * Decrements the interactions count for a Greenitor.
     * It checks if the decrement leads to removing any existing badges based on interactions.
     * If a badge is removed, it is deleted from the user's badges.
     *
     * @param username the username of the Greenitor whose interactions are to be decremented
     * @return a ResponseMessage indicating the result of the interaction decrement
     */
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

    /**
     * Retrieves all Greenitors in the system.
     *
     * @return a list of all Greenitor objects
     */
    public List<Greenitor> getAllGreenitors() {
        List<Greenitor> users = greenitorRepository.findAll();
        for (Greenitor user : users) {
            user.setBadges(badgeRepository.findBadgesByGreenitorId(user.getId()));
        }

        return users;
    }

    public ResponseMessage updateGreenitor(String username, UpdateGreenitorDTO greenitor){
        Greenitor toUpdate = greenitorRepository.findByUsername(username);
        if (toUpdate == null){
            throw new UserNotFoundException("User not found");
        }
        if (!greenitor.getEmail().isEmpty() && !greenitor.getEmail().equals(toUpdate.getEmail())){
            toUpdate.setEmail(greenitor.getEmail());
        }
        if(greenitor.getPassword()!=null && !greenitor.getPassword().isEmpty()){
            toUpdate.setPassword(greenitor.getPassword());
        }
        if(greenitor.getImage()!=null && !greenitor.getImage().isEmpty()){
            toUpdate.setImage(greenitor.getImage());
        }
        greenitorRepository.save(toUpdate);
        return new ResponseMessage( "User successfully updated" );

    }
}
