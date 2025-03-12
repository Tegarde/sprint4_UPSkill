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

    /**
     * Converts a Greenitor entity to a GreenitorDTO.
     *
     * @param greenitor The Greenitor entity to convert.
     * @return A GreenitorDTO populated with data from the provided Greenitor entity.
     */
    public static GreenitorDTO fromGreenitor(Greenitor greenitor) {
        return new GreenitorDTO(greenitor.getUsername(), greenitor.getEmail(), greenitor.getRole(), greenitor.getInteractions(), greenitor.getImage(), greenitor.getBadges().stream().map(BadgeMapper::toDescriptionDTO).toList());
    }

    /**
     * Converts a Greenitor entity to a GreenitorSearchDTO.
     *
     * @param greenitor The Greenitor entity to convert.
     * @return A GreenitorSearchDTO populated with data from the provided Greenitor entity.
     */
    public static GreenitorSearchDTO toGreenitorSearchDTO(Greenitor greenitor) {
        return new GreenitorSearchDTO(greenitor.getUsername(), greenitor.getEmail(), greenitor.getImage(), greenitor.getInteractions() , greenitor.getBadges().stream().map(BadgeMapper::toDescriptionDTO).toList());
    }
}
