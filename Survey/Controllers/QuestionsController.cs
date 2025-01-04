using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Survey.Data;
using Survey.DTOs;
using Survey.Models;

namespace Survey.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly SurveyContext _context;

        public QuestionsController(SurveyContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var surveyContext = _context.Questions.Include(q => q.SurveyT);
            return View(await surveyContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.SurveyT)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["SurveyTId"] = new SelectList(_context.Surveys, "Id", "Title");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SurveyTId,Text,Type")] QuestionDto question)
        {
            if (ModelState.IsValid)
            {
                Question newQuestion = new Question(question.Text, question.Type, question.SurveyTId);

                _context.Add(newQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SurveyTId"] = new SelectList(_context.Surveys, "Id", "Title", question.SurveyTId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["SurveyTId"] = new SelectList(_context.Surveys, "Id", "Title", question.SurveyTId);
            QuestionDto questionDto = new QuestionDto(question.Id,question.SurveyTId,question.Text,question.Type);
            return View(questionDto);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SurveyTId,Text,Type")] QuestionDto question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Question curr = await _context.Questions.FindAsync( id);
                    curr.UpdateText(question.Text);
                    curr.UpdateType(question.Type);
                    curr.UpdateSurveyTId(question.SurveyTId);
                    _context.Update(curr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SurveyTId"] = new SelectList(_context.Surveys, "Id", "Title", question.SurveyTId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.SurveyT)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }

        public IActionResult ViewAnswers(int id)
        {
            // Fetch answers for the given question
            var answers = _context.Answers
                                  .Where(a => a.QuestionId == id)
                                  .Include(a => a.User)
                                  .ToList();
            

            if (!answers.Any())
            {
                TempData["Message"] = "No answers found for this question.";
            }

            ViewBag.QuestionText = _context.Questions
                                           .Where(q => q.Id == id)
                                           .Select(q => q.Text)
                                           .FirstOrDefault();

            return View(answers);
        }

    }
}
