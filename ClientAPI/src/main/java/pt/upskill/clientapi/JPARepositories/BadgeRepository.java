package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Badge;

@Repository
public interface BadgeRepository extends JpaRepository<Badge, Integer> {

}
