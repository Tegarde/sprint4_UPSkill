package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.Models.Badge;

import java.util.List;

public interface BadgeDAO {
    Badge createBadge(Badge badge);

    List<Badge> getAllBadges();
}
