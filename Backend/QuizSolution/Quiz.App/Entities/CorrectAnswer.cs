namespace Quiz.App.Entities
{
    public class CorrectAnswer
    {
        public int Id { get; set; }
        public int MultipleChoiceQuestionId { get; set; }
        public int QuestionAnswerId { get; set; }

        public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }
        public virtual QuestionAnswer QuestionAnswer { get; set; }
    }
}