namespace ForumAPI.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
        
        public UserNotFoundException() : base() { }
    }
}
