package pt.upskill.clientapi.DTOs;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class GreenitorSearchDTO {
    public String username;

    public String email;

    public String image;

    private List<BadgeDescriptionDTO> badges;
}
