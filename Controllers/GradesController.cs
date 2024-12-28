using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsTranscript.Models;

namespace StudentsTranscript.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("performance/{studentId}")]
        public async Task<ActionResult<object>> GetStudentPerformance(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentID == studentId) // Filter by student
                .ToListAsync();

            if (!grades.Any())
            {
                return NotFound("No grades found for this student.");
            }

            var report = new
            {
                AverageGrade = grades.Average(g => g.Grade),
                MaxGrade = grades.Max(g => g.Grade),
                MinGrade = grades.Min(g => g.Grade)
            };

            return Ok(report);
        }

        // GET: api/Grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grades>>> GetGrades()
        {
            var grades = await _context.Grades
                .Include(s => s.Student)
                .Select(g => new
                {
                    g.ID,
                    g.Subject,
                    g.Grade,
                    Student = new
                    {
                        g.Student.ID,
                        g.Student.Name,
                        g.Student.Email
                    }
                })
                .ToListAsync();

            return Ok(grades);
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grades>> GetGrades(int id)
        {
            var grades = await _context.Grades.FindAsync(id);

            if (grades == null)
            {
                return NotFound();
            }

            return grades;
        }

        // PUT: api/Grades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrades(int id, Grades grades)
        {
            if (id != grades.ID)
            {
                return BadRequest();
            }

            _context.Entry(grades).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Grades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Grades>> PostGrades(Grades grades)
        {
            _context.Grades.Add(grades);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrades", new { id = grades.ID }, grades);
        }

        // DELETE: api/Grades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrades(int id)
        {
            var grades = await _context.Grades.FindAsync(id);
            if (grades == null)
            {
                return NotFound();
            }

            _context.Grades.Remove(grades);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradesExists(int id)
        {
            return _context.Grades.Any(e => e.ID == id);
        }
    }
}
