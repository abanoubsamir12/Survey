using Microsoft.EntityFrameworkCore;
using Survey.Models;
namespace Survey.Data
{
    public class SurveyContext : DbContext
    {
        public SurveyContext(DbContextOptions<SurveyContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<SurveyT> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserSurvey> UserSurveys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();


            // Configure relationships
            modelBuilder.Entity<Question>()
                .HasOne(q => q.SurveyT)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SurveyTId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);


            modelBuilder.Entity<UserSurvey>()
            .HasKey(us => new { us.UserId, us.SurveyTId });

            modelBuilder.Entity<UserSurvey>()
                .HasOne(us => us.user)
                .WithMany(u => u.UserSurveys)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSurvey>()
                .HasOne(us => us.surveyT)
                .WithMany(s => s.UserSurveys)
                .HasForeignKey(us => us.SurveyTId);
        }
    }
}
