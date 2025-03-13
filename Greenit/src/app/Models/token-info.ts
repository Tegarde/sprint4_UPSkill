/**
 * Represents the decoded information from an authentication token.
 */
export interface TokenInfo {
    /** The username of the authenticated user */
    username: string;

    /** The role of the user (e.g., "Admin", "User") */
    role: string;
}
