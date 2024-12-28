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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Students>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudents(int id)
        {
            var students = await _context.Students.FindAsync(id);

            if (students == null)
            {
                return NotFound();
            }

            return students;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudents(int id, Students students)
        {
            if (id != students.ID)
            {
                return BadRequest();
            }

            _context.Entry(students).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Students>> PostStudents(Students students)
        {
            _context.Students.Add(students);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudents", new { id = students.ID }, students);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudents(int id)
        {
            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }

            _context.Students.Remove(students);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Students>>> FilterStudents(
            [FromQuery]string? name,
            [FromQuery]int? minAge,
            [FromQuery]string? sortBy)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }
            if (minAge.HasValue)
            {
                query = query.Where(s => DateTime.Now.Year - s.DateOfBirth.Year >=
                minAge.Value);
            }
            if(!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy switch
                {
                    "name" => query.OrderBy(s => s.Name),
                    "age" => query.OrderBy(s => s.DateOfBirth),
                    _ => query
                };
            }
            return await query.ToListAsync();
        }
        [HttpGet("with-grades")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentsWithGrades()
        {
            var studentsWithGrades = await _context.Students
                .Include(s => s.Grades) // Load related grades (this will load the collection of grades)
                .Select(s => new
                {
                    s.ID,  // Use the correct property name for the student ID
                    s.Name,
                    s.Email,
                    Grades = s.Grades.Select(g => new
                    {
                        g.Subject,
                        g.Grade  // The property name for grade is "Grade", not "GradeValue"
                    })
                })
                .ToListAsync();

            return Ok(studentsWithGrades);
        }

        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
