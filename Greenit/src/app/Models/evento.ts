import { Comment } from "./comment";

/**
 * Represents an event with details and user interactions.
 */
export interface Evento {
    /** Unique identifier of the event */
    id: number;

    /** Description of the event */
    description: string;

    /** Location where the event will take place */
    location: string;

    /** Date and time of the event */
    date: Date;

    /** Status of the event (e.g., Open, Closed, Canceled) */
    status: number;

    /** Optional URL or path to the event's image */
    image?: string;

    /** List of comments associated with the event */
    comments: Comment[];
}
