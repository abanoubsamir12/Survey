namespace Survey.DTOs
{
    public class SurveyTDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public SurveyTDto() { }
        public SurveyTDto(int id,string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
