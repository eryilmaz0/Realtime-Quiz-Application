using Quiz.App.Entities;

namespace Quiz.App.SignalR.Objects
{
    public class ChatMessage
    {
        public User User { get; set; }
        public string Message { get; set; }


        public ChatMessage()
        {
            
        }


        public ChatMessage(User user, string message)
        {
            this.User = user;
            this.Message = message;
        }
    }
}