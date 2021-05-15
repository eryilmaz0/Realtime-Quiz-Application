namespace Quiz.App.SignalR.Objects
{
    public class HubResult
    {
        public int Result { get; set; }
        public string OppositeConnectionId { get; set; }


        public HubResult(int result)
        {
            Result = result;
        }


        public HubResult(int result, string connectionId)
        {
            Result = result;
            OppositeConnectionId = connectionId;
        }
    }
}