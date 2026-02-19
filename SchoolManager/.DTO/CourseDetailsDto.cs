namespace SchoolManager.DTO
{
    public class CourseDetailsDto : CourseDto
    {
        public List<StudentDto> Students { get; set; } = new();
        public List<ModuleDto> Modules { get; set; } = new();
    }
}
