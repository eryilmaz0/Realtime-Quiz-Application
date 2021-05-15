namespace Quiz.App.ResponseModels
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public ResponseModel(bool success)
        {
            this.Success = success;
        }


        public ResponseModel(bool success, string message):this(success)
        {
            this.Message = message;
        }
    }
}