package pt.upskill.clientapi;

import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Component;

import java.io.IOException;

@Component
public class MyStartupListener {

    private static final String SWAGGER_UI_URL = "http://localhost:8080/swagger-ui.html"; // Update if running on a different port

    @EventListener(ApplicationReadyEvent.class)
    public void openSwaggerUI() {
        try {
            String os = System.getProperty("os.name").toLowerCase();

            if (os.contains("win")) {
                System.out.println("🖥️ Detected OS: Windows. Opening Swagger UI...");
                Runtime.getRuntime().exec("cmd /c start " + SWAGGER_UI_URL); // Windows
            } else if (os.contains("mac")) {
                System.out.println("🍏 Detected OS: macOS. Opening Swagger UI...");
                Runtime.getRuntime().exec("open " + SWAGGER_UI_URL); // macOS
            } else if (os.contains("nix") || os.contains("nux") || os.contains("linux")) {
                System.out.println("🐧 Detected OS: Linux. Opening Swagger UI...");
                Runtime.getRuntime().exec("xdg-open " + SWAGGER_UI_URL); // Linux
            } else {
                System.out.println("❌ Unsupported OS detected (" + os + "). Cannot auto-open Swagger UI.");
            }

            System.out.println("✅ Swagger UI should open automatically in your browser.");
        } catch (IOException e) {
            System.err.println("❌ Failed to open Swagger UI: " + e.getMessage());
        }
    }
}