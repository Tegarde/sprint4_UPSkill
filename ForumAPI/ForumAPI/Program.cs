
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyMethod()
                              .AllowAnyHeader().
                              AllowCredentials();
                    });
            });



            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ForumAPI", Version = "v1" });

            });

            builder.Services.AddScoped<GreenitorDAO, GreenitorClient>();
            builder.Services.AddScoped<PostDAO, PostService>();
            builder.Services.AddScoped<EventDAO, EventService>();
            builder.Services.AddScoped<CommentDAO, CommentService>();
            builder.Services.AddScoped<CategoryDAO, CategoryClient>();
            builder.Services.AddScoped<FileUploadService>();

            var app = builder.Build();

            app.UseStaticFiles();    //Serve files from wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                        Path.Combine(builder.Environment.ContentRootPath, "uploads")),
                RequestPath = "/Uploads"
            });

            app.UseCors("AllowSpecificOrigins");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
