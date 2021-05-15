using Quiz.App.Entities;

namespace Quiz.App.SignalR.Objects
{
    public class ConnectedUser
    {
        public User User { get; }
        public string ConnectionId { get; }



        public ConnectedUser(User user, string connectionId)
        {
            this.User = user;
            this.ConnectionId = connectionId;
        }


        public ConnectedUser()
        {

        }

    }
}