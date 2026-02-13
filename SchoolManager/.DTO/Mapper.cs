using SchoolManager.Data;

namespace SchoolManager.DTO
{
    public class Mapper
    {
        public StudentDto MapEntityToDto(Student entity)
        {
            StudentDto dto = new StudentDto()
            {
                Id = entity.StudentId,
                Name = entity.Name,
                Surname = entity.Surname,
                Courses = entity.Enrollments?
                    .Where(e => e.Course != null)
                    .Select(x => x.Course)
                    .ToList()
                    .ConvertAll(MapEntityToDto),
            };
            return dto;
        }

        public CourseDto MapEntityToDto(Course entity)
        {
            CourseDto dto = new CourseDto()
            {
                Id = entity.CourseId,
                Title = entity.Title
            };
            return dto;
        }

        public Student MapDtoToEntity(StudentDto dto)
        {
            return new Student()
            {
                StudentId = dto.Id,
                Name = dto.Name,
                Surname = dto.Surname
            };
        }
    }
}
