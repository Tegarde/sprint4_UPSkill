using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ForumAPI.Tests.Services
{
    public class PostServiceTests
    {
        private readonly DataContext _context;
        private readonly Mock<GreenitorDAO> _mockGreenitorClient;
        private readonly Mock<CategoryDAO> _mockCategoryClient;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);
            _mockGreenitorClient = new Mock<GreenitorDAO>();
            _mockCategoryClient = new Mock<CategoryDAO>();
            _postService = new PostService(_context, _mockGreenitorClient.Object, _mockCategoryClient.Object);
        }

        [Fact]
        public async Task GetAllPosts_ReturnsAllPosts()
        {
            // Arrange
            var posts = new List<Post> { new Post(), new Post() };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetAllPosts();

            // Assert
            Assert.Equal(posts.Count, result.Count);
        }

        [Fact]
        public async Task GetPostById_ExistingPost_ReturnsPost()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId };
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetPostById(postId);

            // Assert
            Assert.Equal(postId, result.Id);
        }

        [Fact]
        public async Task GetPostById_NonExistingPost_ThrowsKeyNotFoundException()
        {
            // Arrange
            var postId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _postService.GetPostById(postId));
        }

        [Fact]
        public async Task GetPostsByUser_ExistingUser_ReturnsPosts()
        {
            // Arrange
            var username = "testuser";
            var posts = new List<Post> { new Post { CreatedBy = username, Status = true } };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetPostsByUser(username);

            // Assert
            Assert.Equal(posts.Count, result.Count);
        }

        [Fact]
        public void GetPostSortedByDate_ReturnsPostsSortedByDate()
        {
            // Arrange
            var posts = new List<Post>
    {
        new Post
        {
            CreatedAt = DateTime.Now.AddDays(-1),
            Status = true,
            Category = "General",
            Content = "Content 1",
            CreatedBy = "User1",
            Title = "Title 1"
        },
        new Post
        {
            CreatedAt = DateTime.Now,
            Status = true,
            Category = "General",
            Content = "Content 2",
            CreatedBy = "User2",
            Title = "Title 2"
        }
    };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = _postService.GetPostSortedByDate();

            // Assert
            Assert.Equal(posts.OrderByDescending(p => p.CreatedAt).ToList(), result);
        }


        [Fact]
        public async Task GetTopPostsByInteractions_ReturnsTopPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { LikedBy = new List<PostLike> { new PostLike() }, Status = true },
                new Post { LikedBy = new List<PostLike> { new PostLike(), new PostLike() }, Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetTopPostsByInteractions(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(posts.OrderByDescending(p => p.LikedBy.Count).First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetFavoritePosts_ExistingUser_ReturnsFavoritePosts()
        {
            // Arrange
            var username = "testuser";
            var post = new Post();
            var favorite = new PostFavorite { Post = post, User = username };
            _context.PostFavorites.Add(favorite);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetFavoritePosts(username);

            // Assert
            Assert.Single(result);
            Assert.Equal(post.Id, result.First().Id);
        }

        [Fact]
        public async Task GetPostsBetweenDates_ValidDates_ReturnsPosts()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-2);
            var endDate = DateTime.UtcNow;
            var posts = new List<Post>
            {
                new Post { CreatedAt = DateTime.UtcNow.AddDays(-1), Status = true },
                new Post { CreatedAt = DateTime.UtcNow.AddDays(-3), Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetPostsBetweenDates(startDate, endDate);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetPostsBetweenDates_InvalidDates_ThrowsArgumentException()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.GetPostsBetweenDates(startDate, endDate));
        }

        [Fact]
        public async Task CreatePost_ValidPost_ReturnsCreatedPost()
        {
            // Arrange
            var post = new Post { CreatedBy = "testuser", Category = "testcategory" };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(post.CreatedBy)).ReturnsAsync(new GreenitorDTO());
            _mockCategoryClient.Setup(c => c.GetCategoryByDescription(post.Category)).ReturnsAsync(new CategoryDTO());

            // Act
            var result = await _postService.CreatePost(post);

            // Assert
            Assert.Equal(post, result);
            Assert.Contains(post, _context.Posts);
        }

        [Fact]
        public async Task UpdatePost_ValidPost_ReturnsUpdatedPost()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId, CreatedBy = "testuser" };
            var updatedPost = new Post { Id = postId, CreatedBy = "testuser", Title = "Updated Title" };
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            var result = await _postService.UpdatePost(postId, updatedPost);

            // Assert
            Assert.Equal(updatedPost.Title, result.Title);
        }

        [Fact]
        public async Task UpdatePost_InvalidPostId_ThrowsArgumentException()
        {
            // Arrange
            var postId = 1;
            var updatedPost = new Post { Id = 2 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.UpdatePost(postId, updatedPost));
        }

        [Fact]
        public async Task UpdatePost_NonExistingPost_ThrowsNotFoundException()
        {
            // Arrange
            var postId = 1;
            var updatedPost = new Post { Id = postId };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _postService.UpdatePost(postId, updatedPost));
        }

        [Fact]
        public async Task AddPostToFavorites_ValidPost_ReturnsOkResult()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            _context.Posts.Add(post);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.AddPostToFavorites(postId, username);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemovePostFromFavorites_ValidPost_ReturnsOkResult()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var favorite = new PostFavorite { PostId = postId, User = username };
            _context.PostFavorites.Add(favorite);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.RemovePostFromFavorites(postId, username);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdatePostStatus_ValidStatus_UpdatesStatus()
        {
            // Arrange
            var postId = 1;
            var post = new Post
            {
                Id = postId,
                Title = "Test Title",
                Content = "Test Content",
                CreatedBy = "testuser",
                Category = "testcategory",
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            await _postService.UpdatePostStatus(postId, false);

            // Assert
            Assert.False(post.Status);
        }


        [Fact]
        public async Task SearchPostsByKeyword_ValidKeyword_ReturnsPosts()
        {
            // Arrange
            var keyword = "test";
            var posts = new List<Post>
            {
                new Post { Title = "test", Status = true },
                new Post { Content = "test", Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.SearchPostsByKeyword(keyword);

            // Assert
            Assert.Equal(posts.Count, result.Count);
        }

        [Fact]
        public async Task GetPostHotnessScore_ValidPost_ReturnsHotnessScore()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId, LikedBy = new List<PostLike> { new PostLike() }, FavoritedBy = new List<PostFavorite> { new PostFavorite() }, Comments = new List<Comment> { new Comment() } };
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetPostHotnessScore(postId);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetHottestPosts_ReturnsHottestPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { LikedBy = new List<PostLike> { new PostLike() }, Status = true },
                new Post { LikedBy = new List<PostLike> { new PostLike(), new PostLike() }, Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetHottestPosts(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(posts.OrderByDescending(p => p.LikedBy.Count).First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetHottestPostsFromLastMonth_ReturnsHottestPosts()
        {
            // Arrange
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            var posts = new List<Post>
            {
                new Post { CreatedAt = DateTime.UtcNow, LikedBy = new List<PostLike> { new PostLike() }, Status = true },
                new Post { CreatedAt = lastMonth.AddDays(-1), LikedBy = new List<PostLike> { new PostLike(), new PostLike() }, Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetHottestPostsFromLastMonth(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(posts.OrderByDescending(p => p.LikedBy.Count).First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetHottestPostsFromLastDay_ReturnsHottestPosts()
        {
            // Arrange
            var lastDay = DateTime.UtcNow.AddDays(-1);
            var posts = new List<Post>
            {
                new Post { CreatedAt = DateTime.UtcNow, LikedBy = new List<PostLike> { new PostLike() }, Status = true },
                new Post { CreatedAt = lastDay.AddDays(-1), LikedBy = new List<PostLike> { new PostLike(), new PostLike() }, Status = true }
            };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetHottestPostsFromLastDay(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(posts.OrderByDescending(p => p.LikedBy.Count).First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetNotificationsByUser_ExistingUser_ReturnsPosts()
        {
            // Arrange
            var username = "testuser";
            var posts = new List<Post> { new Post { CreatedBy = username, Status = true, Interactions = 1 } };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetNotificationsByUser(username);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task ResetPostInteractionCount_ValidPost_ResetsInteractionCount()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId, Interactions = 5 };
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            await _postService.ResetPostInteractionCount(postId);

            // Assert
            Assert.Equal(0, post.Interactions);
        }

        [Fact]
        public async Task GetPostStatisticsByUsername_ExistingUser_ReturnsStatistics()
        {
            // Arrange
            var username = "testuser";
            var post = new Post { CreatedBy = username };
            _context.Posts.Add(post);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetPostStatisticsByUsername(username);

            // Assert
            Assert.Equal(1, result.Posts);
        }

        [Fact]
        public async Task LikePost_ValidPost_ReturnsPostLike()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postLike = new PostLike { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.LikePost(postLike);

            // Assert
            Assert.Equal(postLike, result);
        }

        [Fact]
        public async Task UnlikePost_ValidPost_ReturnsPostLike()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postLike = new PostLike { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.PostLikes.Add(postLike);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.UnlikePost(postLike);

            // Assert
            Assert.Equal(postLike, result);
        }

        [Fact]
        public async Task DislikePost_ValidPost_ReturnsPostDislike()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postDislike = new PostDislike { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.DislikePost(postDislike);

            // Assert
            Assert.Equal(postDislike, result);
        }

        [Fact]
        public async Task UndislikePost_ValidPost_ReturnsPostDislike()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postDislike = new PostDislike { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.PostDislikes.Add(postDislike);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.UndislikePost(postDislike);

            // Assert
            Assert.Equal(postDislike, result);
        }

        [Fact]
        public async Task GetPostInteractionsByUser_ValidPost_ReturnsInteractionType()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postLike = new PostLike { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.PostLikes.Add(postLike);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetPostInteractionsByUser(postId, username);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetLikesAndDislikesByPostId_ValidPost_ReturnsInteractions()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId };
            var postLike = new PostLike { PostId = postId };
            var postDislike = new PostDislike { PostId = postId };
            _context.Posts.Add(post);
            _context.PostLikes.Add(postLike);
            _context.PostDislikes.Add(postDislike);
            _context.SaveChanges();

            // Act
            var result = await _postService.GetLikesAndDislikesByPostId(postId);

            // Assert
            Assert.Equal(1, result.Likes);
            Assert.Equal(1, result.Dislikes);

        }
        [Fact]
        public async Task GetPostFavoriteByUsername_ValidPost_ReturnsFavoriteStatus()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var post = new Post { Id = postId };
            var postFavorite = new PostFavorite { PostId = postId, User = username };
            _context.Posts.Add(post);
            _context.PostFavorites.Add(postFavorite);
            _context.SaveChanges();
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetPostFavoriteByUsername(postFavorite);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetPostFavoriteByUsername_NonExistingPost_ReturnsZero()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var postFavorite = new PostFavorite { PostId = postId, User = username };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _postService.GetPostFavoriteByUsername(postFavorite);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetPostFavoriteByUsername_NonExistingUser_ThrowsNotFoundException()
        {
            // Arrange
            var postId = 1;
            var username = "testuser";
            var postFavorite = new PostFavorite { PostId = postId, User = username };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(username)).ThrowsAsync(new NotFoundException("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _postService.GetPostFavoriteByUsername(postFavorite));
        }
    }
}
