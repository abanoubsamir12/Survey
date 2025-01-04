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
    public class SurveyTsController : Controller
    {
        private readonly SurveyContext _context;

        public SurveyTsController(SurveyContext context)
        {
            _context = context;
        }

        // GET: SurveyTs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Surveys.ToListAsync());
        }

        // GET: SurveyTs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveyT = await _context.Surveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (surveyT == null)
            {
                return NotFound();
            }

            return View(surveyT);
        }

        // GET: SurveyTs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SurveyTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedAt")] SurveyTDto surveyT)
        {
            if (ModelState.IsValid)
            {
                SurveyT newSurvey = new SurveyT(surveyT.Title,surveyT.Description);
                _context.Add(newSurvey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(surveyT);
        }

        // GET: SurveyTs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveyT = await _context.Surveys.FindAsync(id);
            if (surveyT == null)
            {
                return NotFound();
            }
            SurveyTDto surveyTDto = new SurveyTDto(surveyT.Id,surveyT.Title,surveyT.Description);
            
            return View(surveyTDto);
        }

        // POST: SurveyTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedAt")] SurveyTDto surveyT)
        {
            if (id != surveyT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SurveyT curr = await _context.Surveys.FindAsync(id);
                    curr.UpdateDescription(surveyT.Description);
                    curr.UpdateTitle(surveyT.Title);
              
                    _context.Update(curr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyTExists(surveyT.Id))
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
            return View(surveyT);
        }

        // GET: SurveyTs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveyT = await _context.Surveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (surveyT == null)
            {
                return NotFound();
            }

            return View(surveyT);
        }

        // POST: SurveyTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var surveyT = await _context.Surveys.FindAsync(id);
            if (surveyT != null)
            {
                _context.Surveys.Remove(surveyT);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyTExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }
    }
}
