package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.BadgeDTO;
import pt.upskill.clientapi.DTOs.BadgeDescriptionDTO;
import pt.upskill.clientapi.Models.Badge;

/**
 * Mapper class for converting between Badge entities and Badge Data Transfer Objects (DTOs).
 * Provides methods for converting from and to different DTO representations.
 */
public class BadgeMapper {

    /**
     * Converts a BadgeDTO to a Badge entity.
     *
     * @param dto The BadgeDTO to convert.
     * @return A Badge entity populated with data from the provided DTO.
     */
    public static Badge fromDTO(BadgeDTO dto) {
        return new Badge(dto.getDescription(), dto.getInteractions());
    }

    /**
     * Converts a Badge entity to a BadgeDTO.
     *
     * @param badge The Badge entity to convert.
     * @return A BadgeDTO populated with data from the provided Badge entity.
     */
    public static BadgeDTO toDTO(Badge badge) {
        return new BadgeDTO(badge.getDescription(), badge.getInteractions());
    }

    /**
     * Converts a Badge entity to a BadgeDescriptionDTO.
     *
     * @param badge The Badge entity to convert.
     * @return A BadgeDescriptionDTO populated with data from the provided Badge entity.
     */
    public static BadgeDescriptionDTO toDescriptionDTO(Badge badge) {
        return new BadgeDescriptionDTO(badge.getDescription(), badge.getImage());
    }
}