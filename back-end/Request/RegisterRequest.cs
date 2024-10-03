namespace back_end.Request
{
    public class LoginRequest
    {
        public required string PlayerName { get; set; }
        public required string Password { get; set; }
    }
}