using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Data
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public List<Enrollment>? Enrollments { get; set; } //nullable

    }
}
