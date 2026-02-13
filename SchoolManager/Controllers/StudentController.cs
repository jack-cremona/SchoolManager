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
        private readonly ILogger<StudentController> _logger;
        private readonly Mapper _mapper;
        public StudentController(SchoolDbContext ctx, ILogger<StudentController> logger, Mapper mapper)
        {
            _ctx = ctx;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Student
        [HttpGet]
        public IActionResult GetAll()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex); //dipendenza per il logging
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var student = _ctx.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
                return NotFound();
            return Ok(_mapper.MapEntityToDto(student)); //utilizzo del mapper per convertire l'entità in DTO
        }

        // GET: api/Student/Details
        [HttpGet]
        [Route("Details")]
        public IActionResult GetAllWithDetails()
        {
            List<Student> result = _ctx.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).ToList();
            List<StudentDto> students = result.ConvertAll(_mapper.MapEntityToDto);
            return Ok(students);
        }


        [HttpPost]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            Student student = new Student()
            {
                StudentId = 0,
                Name = dto.Name,
                Surname = dto.Surname
            };
            _ctx.Students.Add(student);
            if (_ctx.SaveChanges() == 1) //chiusura della transazione
                return NoContent();
            else
                return UnprocessableEntity();

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StudentDto dto)
        {
            var student = _ctx.Students.SingleOrDefault(s => s.StudentId == id); //riga del database. riconosce da solo che la riga di db è modificata
            if (student == null)
                return BadRequest();

            student.Name = dto.Name;
            student.Surname = dto.Surname;
            if (_ctx.SaveChanges() == 1) //chiusura della transazione
                return NoContent();
            else
                return UnprocessableEntity();
        }

        // DELETE: api/Student/1
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var student = _ctx.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
                return BadRequest();
            _ctx.Students.Remove(student);
            if (_ctx.SaveChanges() == 1) //chiusura della transazione
                return NoContent();
            else
                return UnprocessableEntity();
        }
    }
}
