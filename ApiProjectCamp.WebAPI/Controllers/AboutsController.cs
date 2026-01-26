using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Dtos.AboutDtos;
using ApiProjectCamp.WebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public AboutsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult AboutList()
        {
            var Abouts = _context.Abouts.ToList();
            return Ok(Abouts);
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {

            var value = _mapper.Map<About>(createAboutDto);
            _context.Abouts.Add(value);
            _context.SaveChanges();
            return Ok("Hakkımda alanı ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var About = _context.Abouts.Find(id);
            _context.Abouts.Remove(About);
            _context.SaveChanges();
            return Ok("Hakkımda alanı silme işlemi başarılı");
        }
        [HttpGet("GetAbout")]
        public IActionResult GetAbout(int id)
        {
            var About = _context.Abouts.Find(id);
            return Ok(About);
        }
        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var value = _mapper.Map<About>(updateAboutDto);
            _context.Abouts.Update(value);
            _context.SaveChanges();
            return Ok("Hakkımda Alanı güncelleme işlemi başarılı");
        }
    }
}
