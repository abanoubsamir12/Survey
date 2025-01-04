namespace Survey.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public int SurveyTId { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public QuestionDto() { }
        public QuestionDto(int id, int surveyTId, string text, string type)
        {
            Id = id;
            SurveyTId = surveyTId;
            Text = text;
            Type = type;
        }
    }
}
