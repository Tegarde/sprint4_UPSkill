/**
 * Represents the data required to change a user's password.
 */
export interface ChangePassword {
    /** The username of the user requesting the password change */
    username: string;

    /** The user's current password */
    currentPassword: string;

    /** The new password the user wants to set */
    newPassword: string;
}
