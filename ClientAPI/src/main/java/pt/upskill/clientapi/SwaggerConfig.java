package pt.upskill.clientapi;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.info.Info;
import org.springframework.context.annotation.Configuration;

@Configuration
@OpenAPIDefinition(
        info = @Info(
                title = "ClientAPI (SpringBoot)",
                version = "1.0.0",
                description = "API for managing client operations"
        )
)
public class SwaggerConfig {
}