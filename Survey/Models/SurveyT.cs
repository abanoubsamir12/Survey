using System.ComponentModel.DataAnnotations;

namespace Survey.Models
{
    public class SurveyT
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; private set; }
        public void UpdateTitle(string title) {  Title = title; }
        public string Description { get; private set; }
        public void UpdateDescription(string description) { Description = description; }

        [Required]
        public DateTime CreatedAt { get; private set; }

        // Navigation property
        public ICollection<Question> Questions { get; private set; }
        public ICollection<UserSurvey> UserSurveys { get; set; }

        public SurveyT(string title, string description)
        {
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;
            Questions = new List<Question>();
        }
    }
}
