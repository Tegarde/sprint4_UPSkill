package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.UserRepository;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

@Service
public class GreenitorService implements GreenitorDAO {

    private final UserRepository userRepository;

    public GreenitorService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }


    @Override
    public Greenitor registerUser(Greenitor greenitor) {
        if (userRepository.existsByEmail(greenitor.getEmail()) || userRepository.existsByUsername(greenitor.getUsername())) {
            throw new IllegalArgumentException("User with email " + greenitor.getEmail() + " already exists");
        }

        return userRepository.save(greenitor);
    }

    @Override
    public Token loginUser(LoginDTO loginDTO) {
        Greenitor greenitor = userRepository.findByEmail(loginDTO.getEmail());
        if (greenitor == null) {
            throw new IllegalArgumentException("User with email " + loginDTO.getEmail() + " does not exist");
        }

        if (!greenitor.getPassword().equals(loginDTO.getPassword())) {
            throw new IllegalArgumentException("Invalid password");
        }

        return new Token(greenitor.getUsername(), greenitor.getRole());
    }


}
