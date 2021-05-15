using Microsoft.EntityFrameworkCore;
using Quiz.App.Entities;

namespace Quiz.App.DbContext
{
    public class QuizDbContext : EfDbContextBase
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\Eren;Database=QuizProjectDatabase;Trusted_Connection=True;MultipleActiveResultSets=True");
        }


        public DbSet<PictureQuestion> PictureQuestions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<CorrectAnswer> CorrectAnswers { get; set; }
    }
}