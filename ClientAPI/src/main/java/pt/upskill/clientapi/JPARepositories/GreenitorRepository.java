package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Badge;
import pt.upskill.clientapi.Models.Greenitor;

import java.util.List;

@Repository
public interface GreenitorRepository extends JpaRepository<Greenitor, Integer> {
    boolean existsByEmail(String email);

    boolean existsByUsername(String username);

    Greenitor findByEmail(String email);

    Greenitor findByUsername(String username);

}
