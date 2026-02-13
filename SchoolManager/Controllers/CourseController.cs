using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<CourseController> _logger;
        private readonly Mapper _mapper;

        public CourseController(SchoolDbContext ctx, ILogger<CourseController> logger, Mapper mapper)
        {
            _ctx = ctx;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _ctx.Courses;
                var courses = result.Select(c => new CourseDto()
                {
                    Id = c.CourseId,
                    Title = c.Title
                });
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var courses = _ctx.Courses.SingleOrDefault(c => c.CourseId == id);
            if (courses == null)
                return NotFound();
            return Ok(_mapper.MapEntityToDto(courses));
        }

        [HttpGet]
        [Route("Details")]
        public IActionResult GetAllWithDetails()
        {
            List<Course> result = _ctx.Courses.Include(c => c.Enrollments).ThenInclude(e => e.Student).ToList();
            List<CourseDto> cources = result.ConvertAll(_mapper.MapEntityToDto);
            return Ok(cources);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CourseDto dto)
        {
            Course course = new Course()
            {
                CourseId = 0,
                Title = dto.Title
            };
            _ctx.Courses.Add(course);
            if (_ctx.SaveChanges() == 1)
                return NoContent();
            else
                return UnprocessableEntity();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CourseDto dto)
        {
            var course = _ctx.Courses.SingleOrDefault(c => c.CourseId == id);
            if (course == null)
                return NotFound();
            course.Title = dto.Title;
            if (_ctx.SaveChanges() == 1)
                return NoContent();
            else
                return UnprocessableEntity();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var course = _ctx.Courses.SingleOrDefault(c => c.CourseId == id);
            if (course == null)
                return NotFound();
            _ctx.Courses.Remove(course);
            if (_ctx.SaveChanges() == 1)
                return NoContent();
            else
                return UnprocessableEntity();
        }
    }
}
