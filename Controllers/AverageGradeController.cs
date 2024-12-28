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
    public class AverageGradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AverageGradeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<Grades>> GetAverageGrade(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var grades = await _context.Grades
                .Where(g => g.StudentID == id)
                .ToListAsync();
            if (!grades.Any())
            {
                return Ok(grades);
            }
            var averageGrade = grades.Average(g => g.Grade);

            return Ok(new
            {
                StudentID = student.ID,
                StudentName = student.Name,
                AverageGrade = averageGrade
            });
        }
    }
}
