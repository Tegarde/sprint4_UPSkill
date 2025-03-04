package pt.upskill.clientapi.JPARepositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import pt.upskill.clientapi.Models.Category;

@Repository
public interface CategoryRepository extends JpaRepository<Category, Integer> {
    boolean existsByDescription(String description);

    Category findByDescription(String description);
}
