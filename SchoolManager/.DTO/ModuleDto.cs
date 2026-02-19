using SchoolManager.Data;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.DTO
{
    public class ModuleDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int CourseId { get; set; }   //Fk verso Course

        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }  //Fk verso Subject

        [MinLength(2)]
        public required string Title { get; set; }

        
    }
}
