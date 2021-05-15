namespace Quiz.App.SignalR.Objects
{
    public class PictureQuestionModel
    {
        public byte Id { get; set; }
        public string PictureName { get; set; }



        public PictureQuestionModel(byte id, string pictureName)
        {
            this.Id = id;
            this.PictureName = pictureName;
        }
    }
}