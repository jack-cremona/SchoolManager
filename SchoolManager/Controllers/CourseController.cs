using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _ctx.Courses.ToList();
            return Ok(courses.Select(_mapper.MapToDto));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var course = _ctx.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null) return NotFound();
            return Ok(_mapper.MapToDto(course));
        }

        [HttpGet("Details")]
        public IActionResult GetAllWithDetails()
        {
            var courses = _ctx.Courses
                .Include(c => c.Enrollments!).ThenInclude(e => e.Student)
                .Include(c => c.Modules)
                .ToList();

            return Ok(courses.Select(_mapper.MapToDetailsDto));
        }

        //da eccezione
        [HttpGet("{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var course = _ctx.Courses
                .Include(c => c.Enrollments!).ThenInclude(e => e.Student)
                .Include(c => c.Modules)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(course));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CourseDto dto)
        {
            var course = new Course { Title = dto.Title };
            _ctx.Courses.Add(course);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = course.CourseId }, _mapper.MapToDto(course));
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var course = _ctx.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null) return NotFound();

            _ctx.Courses.Remove(course);
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
    }
}