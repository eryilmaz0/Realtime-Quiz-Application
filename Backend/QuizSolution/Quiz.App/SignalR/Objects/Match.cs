using Quiz.App.Entities;

namespace Quiz.App.SignalR.Objects
{
    public class Match
    {
        public ConnectedUser FirstUser { get; set; }
        public ConnectedUser SecondUser { get; set; }
        public bool FirstUserIsReady { get; set; }
        public bool SecondUserIsReady { get; set; }


        public Match(ConnectedUser firstUser, ConnectedUser secondUser)
        {
            this.FirstUser = firstUser;
            this.SecondUser = secondUser;
            this.FirstUserIsReady = false;
            this.SecondUserIsReady = false;
        }


        public Match()
        {
            this.FirstUserIsReady = false;
            this.SecondUserIsReady = false;
        }
    }
}