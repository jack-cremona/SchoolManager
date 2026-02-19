using SchoolManager.Data;

namespace SchoolManager.DTO
{
    public class Mapper
    {
        public StudentDto MapToDto(Student student) => new StudentDto()
        {
            Id = student.StudentId,
            Name = student.Name,
            Surname = student.Surname
        };
        public CourseDto MapToDto(Course course) => new CourseDto()
        {
            Id = course.CourseId,
            Title = course.Title
        };
        public ModuleDto MapToDto(Module module) => new ModuleDto()
        {
            Id = module.ModuleId,
            Title = module.Title,
            CourseId = module.CourseId,
            SubjectId = module.SubjectId
        };
        public SubjectDto MapToDto(Subject subject) => new SubjectDto()
        {
            Id = subject.SubjectId,
            Name = subject.Name
        };
        public TeacherDto MapToDto(Teacher teacher) => new TeacherDto()
        {
            Id = teacher.TeacherId,
            Name = teacher.Name,
            Surname = teacher.Surname
        };
        public StudentDetailsDto MapToDetailsDto(Student student) => new()
        {
            Id = student.StudentId,
            Name = student.Name,
            Surname = student.Surname,
            Courses = student.Enrollments?.Select(e => MapToDto(e.Course!)).ToList() ?? new()
        };

        public CourseDetailsDto MapToDetailsDto(Course course) => new()
        {
            Id = course.CourseId,
            Title = course.Title,
            Students = course.Enrollments?.Select(e => MapToDto(e.Student!)).ToList() ?? new(),
            Modules = course.Modules?.Select(MapToDto).ToList() ?? new()
        };

        public ModuleDetailsDto MapToDetailsDto(Module module) => new()
        {
            Id = module.ModuleId,
            Title = module.Title,
            CourseId = module.CourseId,
            SubjectId = module.SubjectId,
            Course = module.Course != null ? MapToDto(module.Course) : null,
            Subject = module.Subject != null ? MapToDto(module.Subject) : null,
            Teachers = module.Teachers?.Select(MapToDto).ToList() ?? new()
        };

        public SubjectDetailsDto MapToDetailsDto(Subject subject) => new()
        {
            Id = subject.SubjectId,
            Name = subject.Name,
            Modules = subject.Modules?.Select(MapToDto).ToList() ?? new(),
            Teachers = subject.Teachers?.Select(MapToDto).ToList() ?? new()
        };

        public TeacherDetailsDto MapToDetailsDto(Teacher teacher) => new()
        {
            Id = teacher.TeacherId,
            Name = teacher.Name,
            Surname = teacher.Surname,
            Modules = teacher.Modules?.Select(MapToDto).ToList() ?? new(),
            Subjects = teacher.Subjects?.Select(MapToDto).ToList() ?? new()
        };
    }
}
