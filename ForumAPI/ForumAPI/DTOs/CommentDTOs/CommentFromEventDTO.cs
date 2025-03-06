namespace ForumAPI.DTOs.CommentDTOs
{
    public class CommentFromEventDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikedBy { get; set; }
        public int CommentsCounter { get; set; }
    }
}
