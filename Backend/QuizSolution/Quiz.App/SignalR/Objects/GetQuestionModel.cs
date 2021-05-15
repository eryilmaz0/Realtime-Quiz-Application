namespace Quiz.App.SignalR.Objects
{
    public class GetQuestionModel
    {
        public int FirstUserId { get; set; }
        public string FirstUserConnectionId { get; set; }
        public int SecondUserId { get; set; }
        public string SecondUserConnectionId { get; set; }

    }
}