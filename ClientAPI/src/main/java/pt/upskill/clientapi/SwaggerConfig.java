package pt.upskill.clientapi;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.info.Contact;
import io.swagger.v3.oas.annotations.info.Info;
import org.springframework.context.annotation.Configuration;

@Configuration
@OpenAPIDefinition(
        info = @Info(
                title = "ClientAPI (SpringBoot)",
                version = "1.0.0",
                description = "API for managing forum-related operations",
                contact = @Contact(
                        name = "GitHub Repository",
                        url = "https://github.com/Tegarde/sprint4_UPSkill"
                )
        )
)
public class SwaggerConfig {
}