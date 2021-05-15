using System.Collections.Generic;

namespace Quiz.App.Entities
{
    public class MultipleChoiceQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int QuestionAnswerId { get; set; }


        public virtual List<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual List<CorrectAnswer> CorrectAnswers { get; set; }
    }
}