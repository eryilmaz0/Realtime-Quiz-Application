namespace Quiz.App.SignalR.Objects
{
    public class PictureQuestion
    {
        public byte Id { get; set; }
        public string PictureName { get; set; }
        public string EnglishEquivalent { get; set; }


        public PictureQuestion(byte id, string pictureName, string englishEquivalent)
        {
            this.Id = id;
            this.PictureName = pictureName;
            this.EnglishEquivalent = englishEquivalent;
        }


        public PictureQuestion()
        {
            
        }

    }
}