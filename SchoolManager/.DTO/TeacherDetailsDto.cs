namespace SchoolManager.DTO
{
    public class TeacherDetailsDto: TeacherDto
    {
        public List<ModuleDto> Modules { get; set; } = new();
        public List<SubjectDto> Subjects { get; set; } = new();
    }
}
