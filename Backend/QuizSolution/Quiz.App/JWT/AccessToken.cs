using System;

namespace Quiz.App.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}