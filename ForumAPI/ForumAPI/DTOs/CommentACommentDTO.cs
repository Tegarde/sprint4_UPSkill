namespace ForumAPI.DTOs
{
    public class CommentACommentDTO
    {
        public int ParentCommentId { get; set; }

        public string Content { get; set; }

        public string CreatedBy { get; set; }
    }
}
