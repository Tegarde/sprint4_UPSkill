package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.UserRepository;
import pt.upskill.clientapi.Models.Greenitor;

@Service
public class GreenitorService implements GreenitorDAO {

    private final UserRepository userRepository;

    public GreenitorService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }


    @Override
    public Greenitor registerUser(Greenitor greenitor) {
        if (userRepository.existsByEmail(greenitor.getEmail())) {
            throw new IllegalArgumentException(String.format("User with email %s already exists", greenitor.getEmail()));
        }

        if (userRepository.existsByUsername(greenitor.getUsername())) {
            throw new IllegalArgumentException(String.format("User with username %s already exists", greenitor.getUsername()));
        }

        return userRepository.save(greenitor);
    }
}
