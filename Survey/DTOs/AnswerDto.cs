namespace Survey.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Response { get; set; }

        public AnswerDto() { }
        public AnswerDto(int id, int questionId, int userId, string response)
        {
            Id = id;
            QuestionId = questionId;
            UserId = userId;
            Response = response;
        }
    }
}
