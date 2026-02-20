using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public SubjectController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var subjects = _ctx.Subjects.ToList();
            return Ok(subjects.Select(_mapper.MapToDto));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var subject = _ctx.Subjects.FirstOrDefault(s => s.SubjectId == id);
            if (subject == null) return NotFound();
            return Ok(_mapper.MapToDto(subject));
        }

        [HttpGet("Details")]
        public IActionResult GetAllWithDetails()
        {
            var subjects = _ctx.Subjects
                .Include(s => s.Modules)
                .Include(s => s.Teachers)
                .ToList();

            return Ok(subjects.Select(_mapper.MapToDetailsDto));
        }

        [HttpGet("{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var subject = _ctx.Subjects
                .Include(s => s.Modules)
                .Include(s => s.Teachers)
                .FirstOrDefault(s => s.SubjectId == id);

            if (subject == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(subject));
        }

        [HttpPost]
        public IActionResult Create([FromBody] SubjectDto dto)
        {
            var subject = new Subject { Name = dto.Name };
            _ctx.Subjects.Add(subject);
            try
            {
                _ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return CreatedAtAction(nameof(Get), new { id = subject.SubjectId }, _mapper.MapToDto(subject));
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] SubjectDto dto)
        {
            var subject = _ctx.Subjects.FirstOrDefault(s => s.SubjectId == id);
            if (subject == null) return NotFound();

            subject.Name = dto.Name;
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
            var subject = _ctx.Subjects.FirstOrDefault(s => s.SubjectId == id);
            if (subject == null) return NotFound();

            _ctx.Subjects.Remove(subject);
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