using ForumAPI.Data;
using ForumAPI.Interfaces;
using ForumAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ForumAPI (.NET)",
                    Version = "v1",
                    Description = "API for managing forum-related operations",
                    Contact = new OpenApiContact
                    {
                        Name = "GitHub Repository",
                        Url = new Uri("https://github.com/Tegarde/sprint4_UPSkill")
                    }
                });

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
            app.UseStaticFiles();

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
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum API V1");
                    c.RoutePrefix = string.Empty; // Sets Swagger UI as the default homepage
                });

                // Automatically open Swagger UI when the application starts
                OpenBrowser("http://localhost:5000/index.html"); // Adjust port if needed
            }

            // Other middleware configurations
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            // Run the application
            app.Run();
        }

        /// <summary>
        /// Opens the Swagger UI in the default web browser when the app starts.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        private static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open browser: " + ex.Message);
            }
        }
    }
}