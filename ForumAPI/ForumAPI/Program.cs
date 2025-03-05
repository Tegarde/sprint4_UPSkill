
using ForumAPI.Data;
using ForumAPI.Interfaces;
using ForumAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<GreenitorDAO, GreenitorClient>();
            builder.Services.AddScoped<PostDAO, PostService>();
            builder.Services.AddScoped<EventDAO, EventService>();
            builder.Services.AddScoped<CommentDAO, CommentService>();
            builder.Services.AddScoped<CategoryDAO, CategoryClient>();

            var app = builder.Build();

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
