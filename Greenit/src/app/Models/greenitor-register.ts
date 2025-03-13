/**
 * Represents the data required to register a new user (Greenitor).
 */
export interface GreenitorRegister {
    /** The chosen username for the new user */
    username: string;

    /** The email address of the user */
    email: string;

    /** The password for the account */
    password: string;

    /** The profile image file uploaded by the user */
    image: File;
}
