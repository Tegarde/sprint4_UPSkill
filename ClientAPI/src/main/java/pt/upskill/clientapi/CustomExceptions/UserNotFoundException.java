package pt.upskill.clientapi.CustomExceptions;

/**
 * Exception thrown when a user is not found in the system.
 * This exception extends {@link RuntimeException} and is used to
 * indicate that an expected user could not be located.
 */
public class UserNotFoundException extends RuntimeException {

    /**
     * Default constructor for UserNotFoundException.
     * Calls the superclass's default constructor.
     */
    public UserNotFoundException() {
        super();
    }

    /**
     * Constructor for UserNotFoundException with a custom message.
     *
     * @param message the detailed message to be passed to the exception
     */
    public UserNotFoundException(String message) {
        super(message);
    }
}