using System.ComponentModel.DataAnnotations;

namespace SchoolManager.DTO
{
    public class CourseDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [MinLength(2)]
        public required string Title { get; set; }

        public List<StudentDto>? Students { get; set; }
    }
}
