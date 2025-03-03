package pt.upskill.clientapi.Interfaces;

import pt.upskill.clientapi.DTOs.LoginDTO;
import pt.upskill.clientapi.Models.Greenitor;
import pt.upskill.clientapi.Models.Token;

import javax.security.auth.login.AccountNotFoundException;

public interface GreenitorDAO {

    public Greenitor registerUser(Greenitor greenitor);

    public Token loginUser(LoginDTO loginDTO) throws AccountNotFoundException;

}
