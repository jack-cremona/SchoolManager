namespace SchoolManager.DTO
{
    public class StudentDetailsDto: StudentDto
    {
        public List<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}
