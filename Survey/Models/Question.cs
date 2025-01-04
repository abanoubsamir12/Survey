using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Survey.Models
{
    public class Question
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [ForeignKey("SurveyT")]
        public int SurveyTId { get; private set; }
        public void UpdateSurveyTId(int id)
        {
            SurveyTId = id;
        }

        [Required]
        [MaxLength(500)]
        public string Text { get; private set; }
        public void UpdateText(string text) { Text = text; }
        [Required]
        [MaxLength(50)]
        public string Type { get; private set; } // Example: "Text", "Multiple Choice"
        public void UpdateType(string type) { Type = type; }
        // Navigation properties
        public SurveyT SurveyT { get; private set; }
        public ICollection<Answer> Answers { get; private set; }

        public Question(string text, string type, int surveyTId)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Question text cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Question type cannot be null or empty.");

            Text = text;
            Type = type;
            SurveyTId = surveyTId;
            Answers = new List<Answer>();
        }
    }
}