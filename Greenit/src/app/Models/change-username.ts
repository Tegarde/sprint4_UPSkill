/**
 * Represents the data required to change a user's username.
 */
export interface ChangeUsername {
    /** The current username of the user */
    currentUsername: string;

    /** The new username the user wants to change to */
    newUsername: string;
}
