using System;
using Quiz.App.SignalR.Objects;
using System.Collections.Generic;
using System.Linq;


namespace Quiz.App.SignalR
{
    public static class SignalRQuestionManager
    {
        private static readonly List<PictureQuestion> _pictureQuestions = new List<PictureQuestion>();

        static SignalRQuestionManager()
        {
           _pictureQuestions = SetPictureQuestions();
        }




        private static List<PictureQuestion> SetPictureQuestions()
        {
            return new List<PictureQuestion>()
            {
                new PictureQuestion(1, "picture1.jpg", "earth"),
                new PictureQuestion(2, "picture2.jpg", "car"),
                new PictureQuestion(3, "picture3.jpg", "sea"),
                new PictureQuestion(4, "picture4.jpg", "mountain"),
                new PictureQuestion(5, "picture5.jpg", "window"),
                new PictureQuestion(6, "picture6.jpg", "ship"),
                new PictureQuestion(7, "picture7.jpg", "city"),
                new PictureQuestion(8, "picture8.jpg", "map"),
                new PictureQuestion(9, "picture9.jpg", "umbrella"),
                new PictureQuestion(10, "picture10.jpg", "flower"),
                new PictureQuestion(11, "picture11.jpg", "galaxy"),
                new PictureQuestion(12, "picture12.jpg", "keyboard"),
                new PictureQuestion(13, "picture13.jpg", "kitchen"),
                new PictureQuestion(14, "picture14.jpg", "accident"),
                new PictureQuestion(15, "picture15.jpg", "airport"),
                new PictureQuestion(16, "picture16.jpg", "castle"),
                new PictureQuestion(17, "picture17.jpg", "highway"),
                new PictureQuestion(18, "picture18.jpg", "honey"),
                new PictureQuestion(19, "picture19.jpg", "watch"),
                new PictureQuestion(20, "picture20.jpg", "horse"),
                new PictureQuestion(21, "picture21.jpg", "factory"),
                new PictureQuestion(22, "picture22.jpg", "truck"),
                new PictureQuestion(23, "picture23.jpg", "knife"),
                new PictureQuestion(24, "picture24.jpg", "coffee"),
                new PictureQuestion(25, "picture25.jpg", "library"),
                new PictureQuestion(26, "picture26.jpg", "pool"),
                new PictureQuestion(27, "picture27.jpg", "mask"),
                new PictureQuestion(28, "picture28.jpg", "chess"),
                new PictureQuestion(29, "picture29.jpg", "bullet"),
                new PictureQuestion(30, "picture30.jpg", "satellite"),
                new PictureQuestion(31, "picture31.jpg", ""),
                new PictureQuestion(32, "picture32.jpg", ""),
                new PictureQuestion(33, "picture33.jpg", ""),
                new PictureQuestion(34, "picture34.jpg", ""),
                new PictureQuestion(35, "picture35.jpg", ""),
                new PictureQuestion(36, "picture36.jpg", ""),
                new PictureQuestion(37, "picture37.jpg", ""),
                new PictureQuestion(38, "picture38.jpg", ""),
                new PictureQuestion(39, "picture39.jpg", ""),
                new PictureQuestion(40, "picture40.jpg", ""),
                new PictureQuestion(41, "picture41.jpg", ""),
                new PictureQuestion(42, "picture42.jpg", ""),
                new PictureQuestion(43, "picture43.jpg", ""),
                new PictureQuestion(44, "picture44.jpg", ""),
                new PictureQuestion(45, "picture45.jpg", ""),
                new PictureQuestion(46, "picture46.jpg", ""),
                new PictureQuestion(47, "picture47.jpg", ""),
                new PictureQuestion(48, "picture48.jpg", ""),
                new PictureQuestion(49, "picture49.jpg", ""),
                new PictureQuestion(50, "picture50.jpg", ""),
            };

        }




        public static PictureQuestionModel  GetRandomPictureQuestion()
        {
            Random rng = new Random();
            short randonQuestionId = (byte)rng.Next(0, 30);
            return _pictureQuestions.Where(x=>x.Id == randonQuestionId).Select(x=> new PictureQuestionModel(x.Id, x.PictureName)).FirstOrDefault();

        }



        public static bool CheckPictureQuestionAnsver(byte id, string answer)
        {
            var targetQuestion = _pictureQuestions.FirstOrDefault(x => x.Id == id);

            return targetQuestion.EnglishEquivalent == answer.ToLower() ? true : false;


        }
    }
}