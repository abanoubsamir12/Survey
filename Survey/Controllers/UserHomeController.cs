using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survey.Data;
using Survey.DTOs;
using Survey.Models;

namespace Survey.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly SurveyContext _context;

        public UserHomeController(SurveyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int  userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            ViewBag.UserId = userId;
            

            //Fetch already answered surveys
            List<UserSurvey> userSurveys = await _context.UserSurveys.Where(us => us.UserId == userId).Include(s=> s.surveyT).ToListAsync();
            List<SurveyT> answeredSurveys = userSurveys
            .Select(us => us.surveyT)
            .ToList();
            // Fetch surveys from the database
            var surveys = await _context.Surveys
                .Where(s => !answeredSurveys.Contains(s))
                .Select(s =>  new SurveyTDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description
                }).ToListAsync();
         
            return View("~/Views/Users/Home/Index.cshtml",surveys);
        }

        public async Task<IActionResult> StartSurvey(int id)
        {

            // Fetch survey questions
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            var surveyQuestions = survey.Questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Type = q.Type
            }).ToList();
            ViewBag.SurveyId = survey.Id;
            return View("~/Views/Users/Home/AnswerSurvey.cshtml", surveyQuestions);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAnswers(Dictionary<int, string> Answers,int SurveyId)
        {
            if (Answers == null || !Answers.Any())
            {
                return BadRequest("No answers provided.");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int id = int.Parse(userId);

            // Save answers to the database
            foreach (var answer in Answers)
            {
                Console.WriteLine(answer.Key +"  "+  ViewBag.UserId+ "   " + answer.Value);
                Answer ans = new Answer(answer.Key,id,answer.Value);
                _context.Answers.Add(ans);
            }

            UserSurvey us = new UserSurvey(id,SurveyId);
            _context.Add(us);
            

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
