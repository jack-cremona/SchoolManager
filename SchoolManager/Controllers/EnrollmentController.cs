using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;

        public EnrollmentController(SchoolDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("Enroll/{studentId}-{courseId}")]
        public IActionResult EnrollStudent([FromRoute] int studentId, [FromRoute] int courseId)
        {
            var studentExists = _ctx.Students.Any(s => s.StudentId == studentId);
            var courseExists = _ctx.Courses.Any(c => c.CourseId == courseId);

            if (!studentExists || !courseExists)
            {
                return StatusCode(404, "Course or student ID not found");
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                Date = DateTime.UtcNow
            };

            _ctx.Add(enrollment);
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return StatusCode(204);
        }
    }
}
