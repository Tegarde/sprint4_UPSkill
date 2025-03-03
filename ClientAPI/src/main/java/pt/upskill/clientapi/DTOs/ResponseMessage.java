package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents an API response message
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class ResponseMessage {
    /**
     * Response message content
     */
    private String message;
}
