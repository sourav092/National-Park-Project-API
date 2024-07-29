using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPWebAPI_Project_1.Models;
using NPWebAPI_Project_1.Models.DTO;
using NPWebAPI_Project_1.Repository.IRepository;

namespace NPWebAPI_Project_1.Controllers
{
    [Route("api/NationalPark")]
    [ApiController]
    [Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDto = _nationalParkRepository.GetNationalParks().
                Select(_mapper.Map<NationalPark, NationalParkDto>);
            return Ok(nationalParkDto);   //200
        }
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound(); //404
            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto); //200
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
          if(nationalParkDto== null) return BadRequest(); //400
          if(_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park in DB !!!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
          if(!ModelState.IsValid) return BadRequest(); //400
          //Save
            var nationalPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if(!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("",$"Something went wrong while save data : {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // return Ok(); //200
            return CreatedAtRoute("GetNationalPark", new
            { nationalParkId = nationalPark.Id }, nationalPark);  //201
        }
        [HttpPut]
        public IActionResult UpdateNationalpark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while update data : {nationalParkDto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent(); //204
        }
        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_nationalParkRepository.NationalParkExists(nationalParkId))
                return NotFound();
            var natioanlPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if(natioanlPark==null) return NotFound();
            if(!_nationalParkRepository.DeleteNationalPark(natioanlPark))
            {
                ModelState.AddModelError("", $"Something went wrong while delete data : {natioanlPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
