package pt.upskill.clientapi.Controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import pt.upskill.clientapi.DTOs.RegisterUserDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Interfaces.GreenitorDAO;
import pt.upskill.clientapi.Mappers.GreenitorMapper;

@RestController
@RequestMapping("api/greenitor")
public class GreenitorController {

    private final GreenitorDAO service;

    public GreenitorController(GreenitorDAO service) {
        this.service = service;
    }

    @PostMapping
    public ResponseEntity<ResponseMessage> registerUser(@RequestBody RegisterUserDTO greenitor) {
        try {
            service.registerUser(GreenitorMapper.fromRegisterDTO(greenitor));
            return new ResponseEntity<>(new ResponseMessage("User registered successfully"), HttpStatus.CREATED);
        } catch (IllegalArgumentException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }
}
