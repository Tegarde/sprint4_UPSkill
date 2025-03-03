package pt.upskill.clientapi.CustomExceptions;

public class UserNotFoundException extends RuntimeException {

    public UserNotFoundException() {
        super();
    }

    public UserNotFoundException(String message) {
        super(message);
    }
}
