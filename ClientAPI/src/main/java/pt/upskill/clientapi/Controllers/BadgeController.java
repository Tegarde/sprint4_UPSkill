package pt.upskill.clientapi.Controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import pt.upskill.clientapi.DTOs.BadgeDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Interfaces.BadgeDAO;
import pt.upskill.clientapi.Mappers.BadgeMapper;

@RestController
@RequestMapping("api/badges")
public class BadgeController {

    private final BadgeDAO service;

    public BadgeController(BadgeDAO service) {
        this.service = service;
    }

    @PostMapping
    public ResponseEntity<ResponseMessage> createBadge(@RequestBody BadgeDTO dto) {
        try {
            service.createBadge(BadgeMapper.fromDTO(dto));
            return new ResponseEntity<>(new ResponseMessage(String.format("Badge %s created successfully", dto.getDescription())), HttpStatus.CREATED);
        }catch (IllegalArgumentException e) {
            return new ResponseEntity<>(new ResponseMessage(e.getMessage()), HttpStatus.BAD_REQUEST);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }

    @GetMapping
    public ResponseEntity<?> getAllBadges() {
        try {
            return new ResponseEntity<>(service.getAllBadges().stream().map(BadgeMapper::toDTO).toList(), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(new ResponseMessage("Something went wrong"), HttpStatus.BAD_REQUEST);
        }
    }
}
