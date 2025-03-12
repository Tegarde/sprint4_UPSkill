using ForumAPI.Data;
using ForumAPI.Interfaces;
using ForumAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace ForumAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy to allow frontend communication
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });

            // Add DbContext for PostgreSQL database connection
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container
            builder.Services.AddControllers();

            // Enable Swagger/OpenAPI with Annotations
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumAPI", Version = "v1" });

                // Enable Swagger Annotations for controller methods
                c.EnableAnnotations();
            });

            // Register your services for Dependency Injection
            builder.Services.AddScoped<GreenitorDAO, GreenitorClient>();
            builder.Services.AddScoped<PostDAO, PostService>();
            builder.Services.AddScoped<EventDAO, EventService>();
            builder.Services.AddScoped<CommentDAO, CommentService>();
            builder.Services.AddScoped<CategoryDAO, CategoryClient>();
            builder.Services.AddScoped<FileUploadService>();

            // Build the app
            var app = builder.Build();

            // Serve static files from wwwroot and uploads folder
            app.UseStaticFiles();  // Serve files from wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "uploads")),
                RequestPath = "/Uploads"
            });

            // Use CORS policy
            app.UseCors("AllowSpecificOrigins");

            // Configure the HTTP request pipeline in Development environment
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // Enable Swagger
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum API V1");  // Link Swagger UI to the JSON file
                });
            }

            // Other middleware configurations
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
