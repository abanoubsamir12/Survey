using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Models
{
    public class UserSurvey
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User user { get; set; }



        [ForeignKey("SurveyT")]
        public int SurveyTId { get; set; }
        public SurveyT surveyT { get; set; } 
        public UserSurvey() { }

        public UserSurvey(int userId, int surveyTId) { 
            UserId = userId;
            SurveyTId = surveyTId;
        }
    }
}
