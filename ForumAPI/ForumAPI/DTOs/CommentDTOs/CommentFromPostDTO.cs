namespace ForumAPI.DTOs.CommentDTOs
{
    public class CommentFromPostDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikeBy { get; set; }
        public int CommentsCounter { get; set; }
    }
}
