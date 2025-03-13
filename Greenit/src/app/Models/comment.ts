/**
 * Represents a comment on a post.
 */
export interface Comment {
    /** Unique identifier of the comment */
    id: number;

    /** The content of the comment */
    content: string;

    /** Username of the person who created the comment */
    createdBy: string;

    /** Date and time when the comment was created (ISO string format) */
    createdAt: string;

    /** The ID of the post to which the comment belongs */
    postId: number;

    /** Number of users who liked the comment */
    likedBy: number;

    /** Number of replies/comments under this comment */
    commentsCounter: number;
}
