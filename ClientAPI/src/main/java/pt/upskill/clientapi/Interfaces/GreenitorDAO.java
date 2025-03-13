package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.DTOs.UpdateGreenitorDTO;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

import javax.security.auth.login.AccountNotFoundException;
import java.util.List;

/**
 * Interface for Data Access Object (DAO) operations related to the Greenitor entity.
 * Provides methods for managing Greenitor entities and their interactions.
 */
public interface GreenitorDAO {

    /**
     * Registers a new Greenitor (user).
     *
     * @param greenitor The Greenitor object containing the user's data.
     * @return The created Greenitor object.
     */
    Greenitor registerUser(Greenitor greenitor);

    /**
     * Logs in a Greenitor based on the provided login credentials.
     *
     * @param loginDTO The login data transfer object containing the user's login credentials.
     * @return A Token object containing the authentication token for the Greenitor.
     * @throws AccountNotFoundException if the user is not found.
     */
    Token loginUser(LoginDTO loginDTO);

    /**
     * Fetches a Greenitor by their username.
     *
     * @param username The username of the Greenitor.
     * @return The Greenitor object corresponding to the given username.
     */
    Greenitor getGreenitorByUsername(String username);

    /**
     * Increments the number of interactions for a Greenitor identified by their username.
     *
     * @param username The username of the Greenitor whose interactions need to be incremented.
     * @return A ResponseMessage indicating the result of the operation.
     */
    ResponseMessage incrementInteractions(String username);

    /**
     * Decrements the number of interactions for a Greenitor identified by their username.
     *
     * @param username The username of the Greenitor whose interactions need to be decremented.
     * @return A ResponseMessage indicating the result of the operation.
     */
    ResponseMessage decrementInteractions(String username);

    /**
     * Retrieves a list of all Greenitors.
     *
     * @return A list of Greenitor objects.
     */
    List<Greenitor> getAllGreenitors();

    /**
     * Updates the details of an existing Greenitor (user).
     *
     * @param username the username of the Greenitor to update
     * @param greenitor the updated user information
     * @return a ResponseMessage indicating the success of the update
     */
    ResponseMessage updateGreenitor(String username, UpdateGreenitorDTO greenitor);
}