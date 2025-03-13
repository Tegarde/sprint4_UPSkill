/**
 * Represents the credentials required for a user (Greenitor) to log in.
 */
export interface GreenitorLogin {
    /** The email address of the user */
    email: string;

    /** The password for authentication */
    password: string;
}
