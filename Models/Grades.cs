using System.ComponentModel.DataAnnotations;

namespace StudentsTranscript.Models
{
    public class Grades
    {
        [Key]
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string Subject { get; set; }
        public decimal Grade { get; set; }

        public virtual Students Student { get; set; }


    }
}
