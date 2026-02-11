namespace SchoolManager.Data
{
    public class Course
    {
        public int CourseId { get; set; }
        public required string Title { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
        public List<Module>? Modules { get; set; }
    }
}
