package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Greenitor;

@Repository
public interface GreenitorRepository extends JpaRepository<Greenitor, Integer> {
    boolean existsByEmail(String email);

    boolean existsByUsername(String username);

    Greenitor findByEmail(String email);
}
