package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Badge;

import java.util.List;

@Repository
public interface BadgeRepository extends JpaRepository<Badge, Integer> {

    @Query("SELECT b FROM Badge b ORDER BY b.interactions")
    List<Badge> findAllSortedByInteractions();

    @Query("SELECT b FROM Greenitor g JOIN g.badges b WHERE g.id = :greenitorId")
    List<Badge> findBadgesByGreenitorId(@Param("greenitorId") int greenitorId);

    boolean existsByDescription(String description);

    boolean existsByInteractions(int interactions);

    Badge findByDescription(String description);
}
