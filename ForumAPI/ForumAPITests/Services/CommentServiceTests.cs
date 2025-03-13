/*
using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using ForumAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ForumAPITests.Utilities;
using Microsoft.EntityFrameworkCore.InMemory;

namespace ForumAPI.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly DataContext _context;
        private readonly Mock<GreenitorDAO> _mockGreenitorDAO;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);
            _mockGreenitorDAO = new Mock<GreenitorDAO>();
            _commentService = new CommentService(_context, _mockGreenitorDAO.Object);
        }

        [Fact]
        public async Task CommentAComment_ShouldThrowNotFoundException_WhenParentCommentNotFound()
        {
            // Arrange
            var comment = new Comment { ParentCommentId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.CommentAComment(comment));
        }

        [Fact]
        public async Task CommentAComment_ShouldAddComment_WhenParentCommentFound()
        {
            // Arrange
            var parentComment = new Comment { Id = 1, ParentPostId = 1 };
            _context.Comments.Add(parentComment);
            _context.SaveChanges();
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            var comment = new Comment { ParentCommentId = 1, CreatedBy = "user" };

            // Act
            var result = await _commentService.CommentAComment(comment);

            // Assert
            Assert.Equal(parentComment, result.ParentComment);
            Assert.Contains(comment, _context.Comments);
            _mockGreenitorDAO.Verify(g => g.IncrementUserInteractions(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CommentAnEvent_ShouldThrowNotFoundException_WhenEventNotFound()
        {
            // Arrange
            var comment = new Comment { EventId = 1 };
            var events = new List<Event>().AsQueryable();
            var mockEvents = DbSetMockHelper.CreateDbSetMock(events);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.CommentAnEvent(comment));
        }
        
        [Fact]
        public async Task CommentAnEvent_ShouldAddComment_WhenEventFound()
        {
            // Arrange
            var ev = new Event { Id = 1 };
            var events = new List<Event> { ev }.AsQueryable();
            var mockEvents = DbSetMockHelper.CreateDbSetMock(events);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            var comment = new Comment { EventId = 1, CreatedBy = "user" };

            // Act
            var result = await _commentService.CommentAnEvent(comment);

            // Assert
            _mockGreenitorDAO.Verify(g => g.IncrementUserInteractions(It.IsAny<string>()), Times.Once);
            Assert.Equal(ev, result.Event);
        }

        [Fact]
        public async Task CommentAPost_ShouldThrowNotFoundException_WhenPostNotFound()
        {
            // Arrange
            var comment = new Comment { PostId = 1 };
            var posts = new List<Post>().AsQueryable();
            var mockPosts = DbSetMockHelper.CreateDbSetMock(posts);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.CommentAPost(comment));
        }

        [Fact]
        public async Task CommentAPost_ShouldAddComment_WhenPostFound()
        {
            // Arrange
            var post = new Post { Id = 1 };
            var posts = new List<Post> { post }.AsQueryable();
            var mockPosts = DbSetMockHelper.CreateDbSetMock(posts);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            var comment = new Comment { PostId = 1, CreatedBy = "user" };

            // Act
            var result = await _commentService.CommentAPost(comment);

            // Assert
            _mockGreenitorDAO.Verify(g => g.IncrementUserInteractions(It.IsAny<string>()), Times.Once);
            Assert.Equal(post, result.Post);
        }

        [Fact]
        public async Task GetCommentStatisticsByUsername_ShouldReturnStatistics()
        {
            // Arrange
            var username = "user";
            var comments = new List<Comment>().AsQueryable();
            var commentLikes = new List<CommentLike>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);

            // Act
            var result = await _commentService.GetCommentStatisticsByUsername(username);

            // Assert
            Assert.Equal(5, result.Comments);
            Assert.Equal(3, result.LikesInComments);
        }

        [Fact]
        public async Task LikeComment_ShouldThrowNotFoundException_WhenCommentNotFound()
        {
            // Arrange
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.LikeComment(commentLike));
        }

        [Fact]
        public async Task LikeComment_ShouldThrowArgumentException_WhenUserAlreadyLikedComment()
        {
            // Arrange
            var comment = new Comment { Id = 1 };
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment> { comment }.AsQueryable();
            var commentLikes = new List<CommentLike> { commentLike }.AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _commentService.LikeComment(commentLike));
        }

        [Fact]
        public async Task LikeComment_ShouldAddLike_WhenCommentFoundAndUserNotLiked()
        {
            // Arrange
            var comment = new Comment { Id = 1 };
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment> { comment }.AsQueryable();
            var commentLikes = new List<CommentLike>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _commentService.LikeComment(commentLike);

            // Assert
            _mockGreenitorDAO.Verify(g => g.IncrementUserInteractions(It.IsAny<string>()), Times.Once);
            Assert.Equal(comment, result.Comment);
        }

        [Fact]
        public async Task UnLikeComment_ShouldThrowNotFoundException_WhenCommentNotFound()
        {
            // Arrange
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.UnLikeComment(commentLike));
        }

        [Fact]
        public async Task UnLikeComment_ShouldThrowArgumentException_WhenUserNotLikedComment()
        {
            // Arrange
            var comment = new Comment { Id = 1 };
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment> { comment }.AsQueryable();
            var commentLikes = new List<CommentLike>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _commentService.UnLikeComment(commentLike));
        }

        [Fact]
        public async Task UnLikeComment_ShouldRemoveLike_WhenCommentFoundAndUserLiked()
        {
            // Arrange
            var comment = new Comment { Id = 1 };
            var commentLike = new CommentLike { CommentId = 1, User = "user" };
            var comments = new List<Comment> { comment }.AsQueryable();
            var commentLikes = new List<CommentLike> { commentLike }.AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _commentService.UnLikeComment(commentLike);

            // Assert
            _mockGreenitorDAO.Verify(g => g.DecrementUserInteractions(It.IsAny<string>()), Times.Once);
            Assert.Equal(commentLike, result);
        }

        [Fact]
        public async Task GetNumberOfLikesFromCommentId_ShouldThrowNotFoundException_WhenCommentNotFound()
        {
            // Arrange
            var commentId = 1;
            var comments = new List<Comment>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetNumberOfLikesFromCommentId(commentId));
        }

        [Fact]
        public async Task GetNumberOfLikesFromCommentId_ShouldReturnLikeCount_WhenCommentFound()
        {
            // Arrange
            var commentId = 1;
            var comment = new Comment { Id = 1 };
            var comments = new List<Comment> { comment }.AsQueryable();
            var commentLikes = new List<CommentLike>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);

            // Act
            var result = await _commentService.GetNumberOfLikesFromCommentId(commentId);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task GetCommentInteractionsByUser_ShouldThrowUserNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var commentId = 1;
            var username = "user";
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync((GreenitorDTO)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _commentService.GetCommentInteractionsByUser(commentId, username));
        }

        [Fact]
        public async Task GetCommentInteractionsByUser_ShouldThrowNotFoundException_WhenCommentNotFound()
        {
            // Arrange
            var commentId = 1;
            var username = "user";
            var comments = new List<Comment>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetCommentInteractionsByUser(commentId, username));
        }

        [Fact]
        public async Task GetCommentInteractionsByUser_ShouldReturnInteractionCount_WhenUserAndCommentFound()
        {
            // Arrange
            var commentId = 1;
            var username = "user";
            var comments = new List<Comment>().AsQueryable();
            var commentLikes = new List<CommentLike>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);
            var mockCommentLikes = DbSetMockHelper.CreateDbSetMock(commentLikes);
            _mockGreenitorDAO.Setup(g => g.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act
            var result = await _commentService.GetCommentInteractionsByUser(commentId, username);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetCommentsByCommentId_ShouldThrowNotFoundException_WhenCommentNotFound()
        {
            // Arrange
            var commentId = 1;
            var comments = new List<Comment>().AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _commentService.GetCommentsByCommentId(commentId));
        }

        [Fact]
        public async Task GetCommentsByCommentId_ShouldReturnReplies_WhenCommentFound()
        {
            // Arrange
            var commentId = 1;
            var comment = new Comment { Id = 1 };
            var replies = new List<Comment> { new Comment { Id = 2, ParentCommentId = 1 } }.AsQueryable();
            var comments = new List<Comment> { comment }.AsQueryable();
            var mockComments = DbSetMockHelper.CreateDbSetMock(comments);

            // Act
            var result = await _commentService.GetCommentsByCommentId(commentId);

            // Assert
            Assert.Equal(replies.ToList(), result);
        }
    }
}
*/