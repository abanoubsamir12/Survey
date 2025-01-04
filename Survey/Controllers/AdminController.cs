using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survey.Data;
using Survey.Models;

namespace Survey.Controllers
{
    public class AdminController : Controller
    {
        private readonly SurveyContext _context;

        public AdminController(SurveyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddSurvey()
        {
            return RedirectToAction("Create", "SurveyTs"); // Redirect to SurveysController's Create action
        }

        public IActionResult AddQuestion()
        {
            return RedirectToAction("Create", "Questions"); // Redirect to QuestionsController's Create action
        }

        public IActionResult ViewQuestions()
        {
            return RedirectToAction("Index", "Questions"); // Redirect to QuestionsController's Index action
        }
        public IActionResult ListInactiveAdmins()
        {
            var inactiveAdmins = _context.Users
                .Where(u => u.Role == Role.Admin && !u.isActive)
                .ToList();

            return View(inactiveAdmins);
        }
        public async Task<IActionResult> ActivateAdmin(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Role != Role.Admin)
            {
                return NotFound();
            }

            user.UpdateisActive(true); // Activate the admin
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListInactiveAdmins)); // Return to the list of inactive admins
        }
    }
}
