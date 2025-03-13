/**
 * Represents the data required to create a new event.
 */
export interface CreateEvent {
    /** Description of the event */
    description: string;

    /** Location where the event will take place */
    location: string;

    /** Date and time of the event */
    date: Date;

    /** Image file associated with the event */
    image: File;
}
