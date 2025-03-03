package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.GreenitorRepository;
import pt.upskill.clientapi.Models.Greenitor;

@Service
public class GreenitorService implements GreenitorDAO {

    private final GreenitorRepository greenitorRepository;

    public GreenitorService(GreenitorRepository greenitorRepository) {
        this.greenitorRepository = greenitorRepository;
    }


    @Override
    public Greenitor registerUser(Greenitor greenitor) {
        if (greenitorRepository.existsByEmail(greenitor.getEmail())) {
            throw new IllegalArgumentException(String.format("User with email %s already exists", greenitor.getEmail()));
        }

        if (greenitorRepository.existsByUsername(greenitor.getUsername())) {
            throw new IllegalArgumentException(String.format("User with username %s already exists", greenitor.getUsername()));
        }

        return greenitorRepository.save(greenitor);
    }
}
