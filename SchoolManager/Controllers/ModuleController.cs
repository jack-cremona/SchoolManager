using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<ModuleController> _logger;
        private readonly Mapper _mapper;

        public ModuleController(SchoolDbContext ctx, ILogger<ModuleController> logger, Mapper mapper)
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
                var result = _ctx.Modules;
                var modules = result.Select(m => new ModuleDto()
                {
                    Id = m.ModuleId,
                    Title = m.Title,
                    CourseId = m.CourseId,
                    SubjectId = m.SubjectId
                });
                return Ok(modules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var module = _ctx.Modules.SingleOrDefault(m => m.ModuleId == id);
            if (module == null)
                return NotFound();
            return Ok(_mapper.MapEntityToDto(module));
        }

        //da controllare la query
        [HttpGet]
        [Route("Details")]
        public IActionResult GetAllWithDetails()
        {
            List<Module> result = _ctx.Modules.Include(m => m.Course).Include(m => m.Subject).ToList();
            List<ModuleDto> modules = result.ConvertAll(_mapper.MapEntityToDto);
            return Ok(modules);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ModuleDto dto)
        {
            Module module = new Module()
            {
                ModuleId = dto.Id,
                Title = dto.Title,
                CourseId = dto.CourseId,
                SubjectId = dto.SubjectId
            };
            _ctx.Modules.Add(module);
            _ctx.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ModuleDto dto)
        {
            var module = _ctx.Modules.SingleOrDefault(m => m.ModuleId == id);
            if (module == null)
                return NotFound();
            module.Title = dto.Title;
            module.CourseId = dto.CourseId;
            module.SubjectId = dto.SubjectId;
            if (_ctx.SaveChanges() == 1)
                return Ok();
            else
                return UnprocessableEntity();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var module = _ctx.Modules.SingleOrDefault(m => m.ModuleId == id);
            if (module == null)
                return NotFound();
            _ctx.Modules.Remove(module);
            if (_ctx.SaveChanges() == 1)
                return Ok();
            else
                return UnprocessableEntity();
        }
    }
}
