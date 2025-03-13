/**
 * Represents a user's attendance confirmation for an event.
 */
export interface AttendEvent {
    /** The unique identifier of the event the user is attending */
    eventId: number;

    /** The username of the attendee */
    username: string;
}
