package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.DTOs.ResponseMessage;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

import javax.security.auth.login.AccountNotFoundException;

public interface GreenitorDAO {

    Greenitor registerUser(Greenitor greenitor);

    Token loginUser(LoginDTO loginDTO) throws AccountNotFoundException;

    Greenitor getGreenitorByUsername(String username);

    ResponseMessage incrementInteractions(String username);

    ResponseMessage decrementInteractions(String username);
}
