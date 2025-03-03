package pt.upskill.clientapi.Services;

import org.springframework.stereotype.Service;
import pt.upskill.clientapi.CustomExceptions.UserNotFoundException;
import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.JPARepositories.GreenitorRepository;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

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

    @Override

    public Token loginUser(LoginDTO loginDTO) {
        Greenitor greenitor = greenitorRepository.findByEmail(loginDTO.getEmail());

        if (greenitor == null) {
            throw new UserNotFoundException(String.format("User with email %s does not exist", loginDTO.getEmail()));
        }
        if (!greenitor.getPassword().equals(loginDTO.getPassword())) {
            throw new IllegalArgumentException("Invalid password");
        }
        return new Token(greenitor.getUsername(), greenitor.getRole());
    }


}
