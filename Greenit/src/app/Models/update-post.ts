/**
 * Represents the structure of an updated post.
 */
export interface UpdatePost {
    /** Unique identifier of the post */
    id: number;

    /** The updated content of the post */
    content: string;

    /** The updated title of the post */
    title: string;

    /** Username of the person who created the post */
    createdBy: string;
}
