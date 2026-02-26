using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Validation;
using SchoolManager.Data;

namespace SchoolManager.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;

        public AssignmentController(SchoolDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("/AssignTeacherToModule/{teacherId}/{moduleId}")]
        public IActionResult AssignTeacherToModule([FromRoute] int teacherId, [FromRoute] int moduleId)
        {
            var teacher = _ctx.Teachers
                .Include(t => t.Modules)
                .Include(t => t.Subjects)
                .FirstOrDefault(t => t.TeacherId == teacherId);
            
            var module = _ctx.Modules.FirstOrDefault(m => m.ModuleId == moduleId);
            
            if (teacher == null || module == null)
            {
                return StatusCode(404, "Module or teacher ID not found");
            }
            var isTeacherCompatibleWithModule = teacher.Subjects != null && teacher.Subjects.Any(q => q.SubjectId == module.SubjectId);
            if (!isTeacherCompatibleWithModule)
            {
                return StatusCode(400, "module's subject isn't into a teacher's knowledge");
            }

            if (teacher.Modules == null)
                teacher.Modules = new List<Module>();
            
            if (teacher.Modules.Any(m => m.ModuleId == moduleId))
            {
                return StatusCode(400, "Teacher is already assigned to this module");
            }

            teacher.Modules.Add(module);
            
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
