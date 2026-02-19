namespace SchoolManager.DTO
{
    public class SubjectDetailsDto: SubjectDto
    {
        public List<ModuleDto> Modules { get; set; } = new();
        public List<TeacherDto> Teachers { get; set; } = new();
    }
}
