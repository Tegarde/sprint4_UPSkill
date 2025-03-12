package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Greenitor;

import java.util.List;

/**
 * Repository interface for managing Greenitor entities.
 * Provides methods for querying the Greenitor table in the database.
 */
@Repository
public interface GreenitorRepository extends JpaRepository<Greenitor, Integer> {

    /**
     * Checks if a Greenitor with the specified email exists.
     *
     * @param email The email of the Greenitor to check.
     * @return true if a Greenitor with the given email exists, otherwise false.
     */
    boolean existsByEmail(String email);

    /**
     * Checks if a Greenitor with the specified username exists.
     *
     * @param username The username of the Greenitor to check.
     * @return true if a Greenitor with the given username exists, otherwise false.
     */
    boolean existsByUsername(String username);

    /**
     * Retrieves a Greenitor by their email.
     *
     * @param email The email of the Greenitor to retrieve.
     * @return The Greenitor object corresponding to the given email.
     */
    Greenitor findByEmail(String email);

    /**
     * Retrieves a Greenitor by their username.
     *
     * @param username The username of the Greenitor to retrieve.
     * @return The Greenitor object corresponding to the given username.
     */
    Greenitor findByUsername(String username);
}