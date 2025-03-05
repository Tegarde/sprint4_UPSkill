using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        /// <summary>
        /// The posts in the database.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// The events in the database.
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// The comments in the database.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// The likes for comments in the database.
        /// </summary>
        public DbSet<CommentLike> CommentLikes { get; set; }

        /// <summary>
        /// The likes for posts in the database.
        /// </summary>
        public DbSet<PostLike> PostLikes { get; set; }

        /// <summary>
        /// The dislikes for posts in the database.
        /// </summary>
        public DbSet<PostDislike> PostDislikes { get; set; }

        /// <summary>
        /// The favorites for posts in the database.
        /// </summary>
        public DbSet<PostFavorite> PostFavorites { get; set; }

        /// <summary>
        /// The attendances for events in the database.
        /// </summary>  
        public DbSet<Attendance> Attendances { get; set; }

        /// <summary>
        /// Configures the model builder for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<PostLike>()
                .HasKey(pl => new { pl.PostId, pl.User });

            modelBuilder.Entity<PostDislike>()
                .HasKey(pd => new { pd.PostId, pd.User });

            modelBuilder.Entity<PostFavorite>()
                .HasKey(pf => new { pf.PostId, pf.User });

            modelBuilder.Entity<Attendance>()
                .HasKey(at => new { at.EventId, at.User });

            modelBuilder.Entity<CommentLike>()
                .HasKey(cl => new { cl.CommentId, cl.User });
        }
    }
}
