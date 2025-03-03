package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Greenitor;

@Repository
public interface UserRepository extends JpaRepository<Greenitor, Integer> {
}
