using System.ComponentModel.DataAnnotations;

namespace SchoolManager.DTO
{
    public class StudentDto

    {
        [Range(1,int.MaxValue)]
        public int Id { get; set; }

        [MinLength(2)]
        public required string Name { get; set; }

        [MinLength(2)]
        public required string Surname { get; set; }
    }
}
