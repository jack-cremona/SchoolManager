using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;
using System.Net.WebSockets;

namespace SchoolManager.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
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

        [HttpGet("GetAllStudents")]
        public IActionResult GetAll()
        {
            var students = _ctx.Students.ToList();
            return Ok(students.Select(_mapper.MapToDto));
        }

        [HttpGet("GetStudent/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();
            return Ok(_mapper.MapToDto(student));
        }

        [HttpGet("GetAllStudentsWithDetails")]
        public IActionResult GetAllWithDetails()
        {
            var students = _ctx.Students
                .Include(s => s.Enrollments!)
                    .ThenInclude(e => e.Course)
                .ToList();

            return Ok(students.Select(_mapper.MapToDetailsDto));
        }

        [HttpGet("GetStudent/{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var student = _ctx.Students
                .Include(s => s.Enrollments!)
                    .ThenInclude(e => e.Course)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(student));
        }

        [HttpPost("CreateStudent")]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            var student = new Student { Name = dto.Name, Surname = dto.Surname };
            _ctx.Students.Add(student);
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return CreatedAtAction(nameof(Get), new { id = student.StudentId }, _mapper.MapToDto(student));
        }

        


        [HttpPut("UpdateStudent/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StudentDto dto)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();

            student.Name = dto.Name;
            student.Surname = dto.Surname;
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

        [HttpDelete("DeleteStudent/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var student = _ctx.Students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();
            List<Enrollment> enrollments = _ctx.Enrollments.Where(e => e.StudentId == id).ToList();
            foreach (var enrollment in enrollments)
            {
                _ctx.Enrollments.Remove(enrollment);
            }

            _ctx.Students.Remove(student);
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
