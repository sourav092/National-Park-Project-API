using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPWebAPI_Project_1.Models;
using NPWebAPI_Project_1.Models.DTO;
using NPWebAPI_Project_1.Repository.IRepository;

namespace NPWebAPI_Project_1.Controllers
{
    [Route("api/trail")]
    [ApiController]
    [Authorize]
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            var trailDtoList = _trailRepository.GetTrails().
                Select(_mapper.Map<Trail,Traildto>);
            return Ok(trailDtoList);
        }
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            var trailDto = _mapper.Map<Trail, Traildto>(trail);
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] Traildto traildto)
        {
            if (traildto == null) return BadRequest();
            if (_trailRepository.TrailExists(traildto.Name))
            {
                ModelState.AddModelError("", $"Trail in use !!!!{traildto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Traildto, Trail>(traildto);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while save data :{traildto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody] Traildto traildto)
        {
            if (traildto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Trail>(traildto);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while update data :{traildto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data :{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
