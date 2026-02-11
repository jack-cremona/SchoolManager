using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Data
{
    [PrimaryKey(nameof(StudentId), nameof(CourseId))] //primary key composta da StudentId e CourseId
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public required DateTime Date { get; set; }

        [ForeignKey(nameof(StudentId))] //specifica che StudentId è una chiave esterna che fa riferimento alla tabella Student
        public Student? Student { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; }
    }
}
