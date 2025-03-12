package pt.upskill.clientapi.CustomExceptions;

/**
 * Exception thrown when a badge is not found in the system.
 * This exception is a subclass of {@link RuntimeException}, and is used to
 * indicate that an expected badge could not be located.
 */
public class BadgeNotFoundException extends RuntimeException {

    /**
     * Default constructor for BadgeNotFoundException.
     * Calls the superclass's default constructor.
     */
    public BadgeNotFoundException() {
        super();
    }

    /**
     * Constructor for BadgeNotFoundException with a custom message.
     *
     * @param message the detailed message to be passed to the exception
     */
    public BadgeNotFoundException(String message) {
        super(message);
    }
}