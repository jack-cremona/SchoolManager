using Microsoft.EntityFrameworkCore;

namespace SchoolManager.Data
{
    public class Module
    {
        public int Id { get; set; }
        public int CourseId { get; set; }   //Fk verso Course
        public int SubjectId { get; set; }  //Fk verso Subject
        public required string Title { get; set; }
        public Course? Course { get; set; }
        public Subject? Subject { get; set; }
        public List<Teacher>? Teachers { get; set; }
    }
}
