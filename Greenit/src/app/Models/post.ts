import { Comment } from "./comment";

/**
 * Represents a post in the forum.
 */
export interface Post {
    /** Unique identifier of the post */
    id: number;

    /** Title of the post */
    title: string;

    /** Main content of the post */
    content: string;

    /** Username of the person who created the post */
    createdBy: string;

    /** Date and time when the post was created (ISO string format) */
    createdAt: string;

    /** Category under which the post is classified */
    category: string;

    /** Status of the post (e.g., active/inactive) */
    status: boolean;

    /** Number of interactions (likes, dislikes, comments, etc.) */
    interactions: number;

    /** Number of users who liked the post */
    likedBy: number;

    /** Number of users who disliked the post */
    dislikedBy: number;

    /** List of comments associated with the post */
    comments: Comment[];

    /** Optional URL or path to the post's image */
    image?: string;
}
