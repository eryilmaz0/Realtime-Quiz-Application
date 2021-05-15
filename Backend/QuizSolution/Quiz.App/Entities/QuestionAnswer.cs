using System.Collections.Generic;

namespace Quiz.App.Entities
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int MultipleChoiceQuestionId { get; set; }


        public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }
        public virtual List<CorrectAnswer> CorrectAnswers { get; set; }
    }
}