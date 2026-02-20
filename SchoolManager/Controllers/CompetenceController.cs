using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetenceController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;

        public CompetenceController(SchoolDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("AddCompetenceToTeacher/{teacherId}-{subjectId}")]
        public IActionResult AddCompetenceToTeacher([FromRoute] int teacherId, [FromRoute] int subjectId)
        {
            var teacherExists = _ctx.Teachers.Any(t => t.TeacherId == teacherId);
            var subjectExists = _ctx.Subjects.Any(s => s.SubjectId == subjectId);

            if (!teacherExists || !subjectExists)
            {
                return StatusCode(404, "Teacher or subject ID not found");
            }

            var competence = new Competence
            {
                TeacherId = teacherId,
                SubjectId = subjectId
            };

            _ctx.Add(competence);
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
