package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.GreenitorDTO;
import pt.upskill.clientapi.DTOs.GreenitorSearchDTO;
import pt.upskill.clientapi.DTOs.RegisterUserDTO;
import pt.upskill.clientapi.Models.Greenitor;

/**
 * Mapper class for the Greenitor entity
 */
public class GreenitorMapper {
    public static Greenitor fromRegisterDTO(RegisterUserDTO dto) {
        return new Greenitor(dto.getUsername(), dto.getEmail(), dto.getPassword(), dto.getImage());
    }

    public static GreenitorDTO fromGreenitor(Greenitor greenitor) {
        return new GreenitorDTO(greenitor.getUsername(), greenitor.getEmail(), greenitor.getRole(), greenitor.getInteractions(), greenitor.getImage(), greenitor.getBadges().stream().map(BadgeMapper::toDescriptionDTO).toList());
    }

    public static GreenitorSearchDTO toGreenitorSearchDTO(Greenitor greenitor) {
        return new GreenitorSearchDTO(greenitor.getUsername(), greenitor.getEmail(), greenitor.getImage() , greenitor.getBadges().stream().map(BadgeMapper::toDescriptionDTO).toList());
    }
}
