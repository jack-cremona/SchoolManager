using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManager.Data;
using SchoolManager.DTO;

namespace SchoolManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public ModuleController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var modules = _ctx.Modules.ToList();
            return Ok(modules.Select(_mapper.MapToDto));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var module = _ctx.Modules.FirstOrDefault(m => m.ModuleId == id);
            if (module == null) return NotFound();
            return Ok(_mapper.MapToDto(module));
        }

        [HttpGet("Details")]
        public IActionResult GetAllWithDetails()
        {
            var modules = _ctx.Modules
                .Include(m => m.Course)
                .Include(m => m.Subject)
                .Include(m => m.Teachers)
                .ToList();

            return Ok(modules.Select(_mapper.MapToDetailsDto));
        }

        [HttpGet("{id}/Details")]
        public IActionResult GetWithDetails([FromRoute] int id)
        {
            var module = _ctx.Modules
                .Include(m => m.Course)
                .Include(m => m.Subject)
                .Include(m => m.Teachers)
                .FirstOrDefault(m => m.ModuleId == id);

            if (module == null) return NotFound();
            return Ok(_mapper.MapToDetailsDto(module));
        }

        [HttpPost]
        public IActionResult Create([FromBody] ModuleDto dto)
        {
            var module = new Module
            {
                Title = dto.Title,
                CourseId = dto.CourseId,
                SubjectId = dto.SubjectId
            };

            _ctx.Modules.Add(module);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = module.ModuleId }, _mapper.MapToDto(module));
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ModuleDto dto)
        {
            var module = _ctx.Modules.FirstOrDefault(m => m.ModuleId == id);
            if (module == null) return NotFound();

            module.Title = dto.Title;
            module.CourseId = dto.CourseId;
            module.SubjectId = dto.SubjectId;
            _ctx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var module = _ctx.Modules.FirstOrDefault(m => m.ModuleId == id);
            if (module == null) return NotFound();

            _ctx.Modules.Remove(module);
            _ctx.SaveChanges();
            return NoContent();
        }
    }
}