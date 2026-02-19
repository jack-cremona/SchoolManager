using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public TeacherController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var teachers = _ctx.Teachers.ToList();
            return Ok(teachers.Select(_mapper.MapToDto));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var teacher = _ctx.Teachers.FirstOrDefault(t => t.TeacherId == id);
            if (teacher == null) return NotFound();
            return Ok(_mapper.MapToDto(teacher));
        }

        [HttpGet("Details")]
        public IActionResult GetAllWithDetails()
        {
            var teachers = _ctx.Teachers
                .Include(t => t.Modules)
                .Include(t => t.Subjects)
                .ToList();

            return Ok(teachers.Select(_mapper.MapToDetailsDto));
        }

        [HttpGet("{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var teacher = _ctx.Teachers
                .Include(t => t.Modules)
                .Include(t => t.Subjects)
                .FirstOrDefault(t => t.TeacherId == id);

            if (teacher == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(teacher));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TeacherDto dto)
        {
            var teacher = new Teacher { Name = dto.Name, Surname = dto.Surname };
            _ctx.Teachers.Add(teacher);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = teacher.TeacherId }, _mapper.MapToDto(teacher));
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] TeacherDto dto)
        {
            var teacher = _ctx.Teachers.FirstOrDefault(t => t.TeacherId == id);
            if (teacher == null) return NotFound();

            teacher.Name = dto.Name;
            teacher.Surname = dto.Surname;
            _ctx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var teacher = _ctx.Teachers.FirstOrDefault(t => t.TeacherId == id);
            if (teacher == null) return NotFound();

            _ctx.Teachers.Remove(teacher);
            _ctx.SaveChanges();
            return NoContent();
        }
    }
}