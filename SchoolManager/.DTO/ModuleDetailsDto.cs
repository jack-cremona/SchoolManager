namespace SchoolManager.DTO
{
    public class ModuleDetailsDto: ModuleDto
    {
        public CourseDto? Course { get; set; }
        public SubjectDto? Subject { get; set; }
        public List<TeacherDto> Teachers { get; set; } = new();
    }
}
