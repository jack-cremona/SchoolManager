using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;
using System.Net.WebSockets;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        public StudentController(SchoolDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET: api/Student
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _ctx.Students;
            var students = result.Select(s => new StudentDto()
            {
                Id = s.StudentId,
                Name = s.Name,
                Surname = s.Surname
            });
            return Ok(students);
        }


        // GET: api/Student/Details
        [HttpGet]
        [Route("Details")]
        public IActionResult GetAllWithDetails()
        {
            List<Student> result = _ctx.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).ToList();
            List<StudentDto> students = result.ConvertAll(s => new StudentDto()
            {
                Id = s.StudentId,
                Name = s.Name,
                Surname = s.Surname,
                Courses = new Dictionary<int, string>
                (
                    s.Enrollments.Select(e => new KeyValuePair<int, string>
                    (
                        e.Course.CourseId,
                        e.Course.Title
                    )
                ))
            });
            return Ok(students);
        }
    }
}
