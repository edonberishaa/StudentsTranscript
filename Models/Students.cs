using System.ComponentModel.DataAnnotations;

namespace StudentsTranscript.Models
{
    public class Students
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
