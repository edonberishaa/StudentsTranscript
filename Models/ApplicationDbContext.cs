using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace StudentsTranscript.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSet for your tables (entities)
        public DbSet<Students> Students { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
    }
}
