package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.RegisterUserDTO;
import pt.upskill.clientapi.Models.Greenitor;

/**
 * Mapper class for the Greenitor entity
 */
public class GreenitorMapper {
    public static Greenitor fromRegisterDTO(RegisterUserDTO dto) {
        return new Greenitor(dto.getUsername(), dto.getEmail(), dto.getPassword());
    }
}
