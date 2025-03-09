package pt.upskill.clientapi.Mappers;

import pt.upskill.clientapi.DTOs.BadgeDTO;
import pt.upskill.clientapi.DTOs.BadgeDescriptionDTO;
import pt.upskill.clientapi.Models.Badge;

/**
 * Mapper class for the Badge entity
 */
public class BadgeMapper {
    public static Badge fromDTO(BadgeDTO dto) {
        return new Badge(dto.getDescription(), dto.getInteractions());
    }

    public static BadgeDTO toDTO(Badge badge) {
        return new BadgeDTO(badge.getDescription(), badge.getInteractions());
    }

    public static BadgeDescriptionDTO toDescriptionDTO(Badge badge) {
        return new BadgeDescriptionDTO(badge.getDescription(), badge.getImage());
    }
}
