package pt.upskill.clientapi.CustomExceptions;

public class BadgeNotFoundException extends RuntimeException{
    public BadgeNotFoundException() {
        super();
    }
    public BadgeNotFoundException(String message) {
        super(message);
    }
}
