using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Survey.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; private set; }
        public void UpdateQuestionId(int id) {  QuestionId = id; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; private set; }
        public void UpdateUserId(int userId) { UserId = userId; }

        [Required]
        public string Response { get; private set; }
        public void UpdateResponse(string response) { Response = response; }


        [Required]
        public DateTime SubmittedAt { get; private set; }

        // Navigation properties
        public Question Question { get; private set; }
        public User User { get; private set; }

        public Answer(int questionId, int userId, string response)
        {
            this.QuestionId = questionId;
            this.UserId = userId;
            this.Response = response;
            SubmittedAt = DateTime.Now;
        }
    }
}