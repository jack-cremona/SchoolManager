using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly Mapper _mapper;

        public StudentController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _ctx.Students.ToList();
            return Ok(students.Select(_mapper.MapToDto));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();
            return Ok(_mapper.MapToDto(student));
        }

        [HttpGet("Details")]
        public IActionResult GetAllWithDetails()
        {
            var students = _ctx.Students
                .Include(s => s.Enrollments!)
                    .ThenInclude(e => e.Course)
                .ToList();

            return Ok(students.Select(_mapper.MapToDetailsDto));
        }

        [HttpGet("{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var student = _ctx.Students
                .Include(s => s.Enrollments!)
                    .ThenInclude(e => e.Course)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(student));
        }

        [HttpPost]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            var student = new Student { Name = dto.Name, Surname = dto.Surname };
            _ctx.Students.Add(student);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = student.StudentId }, _mapper.MapToDto(student));
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StudentDto dto)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();

            student.Name = dto.Name;
            student.Surname = dto.Surname;
            _ctx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();

            _ctx.Students.Remove(student);
            _ctx.SaveChanges();
            return NoContent();
        }
    }
}
