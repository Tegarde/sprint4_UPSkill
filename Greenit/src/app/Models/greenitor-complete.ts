import { Badge } from "./badge";

/**
 * Represents a complete user profile (Greenitor) with additional details.
 */
export interface GreenitorComplete {
    /** The username of the user */
    username: string;

    /** The email address of the user */
    email: string;

    /** The password for authentication */
    password: string;

    /** The role of the user (e.g., "Admin", "User") */
    role: string;

    /** The number of interactions made by the user (e.g., posts, comments, likes) */
    interactions: number;

    /** URL or path to the user's profile image */
    image: string;

    /** List of badges earned by the user */
    badges: Badge[];
}
