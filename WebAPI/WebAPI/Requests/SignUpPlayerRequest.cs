namespace Habr.Common.Requests
{
    public class SignUpPlayerRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
    }
}