package pt.upskill.clientapi.CustomExceptions;

/**
 * Exception thrown when a category is not found in the system.
 * This exception extends {@link RuntimeException} and is used to
 * indicate that an expected category could not be located.
 */
public class CategoryNotFoundException extends RuntimeException {

    /**
     * Default constructor for CategoryNotFoundException.
     * Calls the superclass's default constructor.
     */
    public CategoryNotFoundException() {
        super();
    }

    /**
     * Constructor for CategoryNotFoundException with a custom message.
     *
     * @param message the detailed message to be passed to the exception
     */
    public CategoryNotFoundException(String message) {
        super(message);
    }
}