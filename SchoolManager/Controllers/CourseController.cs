using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public CourseController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet("GetAllCourses")]
        public IActionResult GetAll()
        {
            var courses = _ctx.Courses.ToList();
            return Ok(courses.Select(_mapper.MapToDto));
        }

        [HttpGet("GetCourse/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var course = _ctx.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null) return NotFound();
            return Ok(_mapper.MapToDto(course));
        }

        [HttpGet("GetAllCoursesWithDetails")]
        public IActionResult GetAllWithDetails()
        {
            var courses = _ctx.Courses
                .Include(c => c.Enrollments!).ThenInclude(e => e.Student)
                .Include(c => c.Modules)
                .ToList();

            return Ok(courses.Select(_mapper.MapToDetailsDto));
        }

        //da eccezione
        [HttpGet("GetCourse/{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var course = _ctx.Courses
                .Include(c => c.Enrollments!).ThenInclude(e => e.Student)
                .Include(c => c.Modules)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(course));
        }

        [HttpPost("CreateCourse")]
        public IActionResult Create([FromBody] CourseDto dto)
        {
            var course = new Course { Title = dto.Title };
            _ctx.Courses.Add(course);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = course.CourseId }, _mapper.MapToDto(course));
        }

        [HttpPut("UpdateCourse/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CourseDto dto)
        {
            var course = _ctx.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null) return NotFound();

            course.Title = dto.Title;
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("DeleteCourse/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var course = _ctx.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
                return NotFound();
            try
            {
                List<Enrollment> enrollments = _ctx.Enrollments.Where(e => e.CourseId == id).ToList();
                foreach (var enrollment in enrollments)
                {
                    _ctx.Enrollments.Remove(enrollment);
                }

                List<Module> modules = _ctx.Modules.Where(m => m.CourseId == id).ToList();
                List<Assignment> assignments;
                foreach (var module in modules)
                {
                    assignments = _ctx.Assignments.Where(a => a.ModuleId == module.ModuleId).ToList();
                    foreach (var assignment in assignments)
                    {
                        _ctx.Assignments.Remove(assignment);
                    }
                    _ctx.Modules.Remove(module);
                }

                _ctx.Courses.Remove(course);

                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            
            return NoContent();
        }
    }
}