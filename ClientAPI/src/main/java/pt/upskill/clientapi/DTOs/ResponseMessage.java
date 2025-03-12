package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents an API response message.
 * This class is used to send a message back to the client in the response body.
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class ResponseMessage {

    /**
     * The content of the response message.
     * This message will be sent as part of the API response to provide feedback to the client.
     */
    private String message;

}