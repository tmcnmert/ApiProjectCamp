using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Dtos.FeatureDtos;
using ApiProjectCamp.WebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public FeaturesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult FeatureList()
        {
            var values = _context.Features.ToList();
            return Ok(_mapper.Map<List<ResultFeatureDto>>(values));//Feature listesini ResultFeatureDto'ya dönüştürüp döndürüyoruz.
        }
        [HttpPost]
        public IActionResult CreateFeature(CreateFeatureDto createFeatureDto)
        {
            var values = _mapper.Map<Feature>(createFeatureDto);//CreateFeatureDto'yı Feature'a dönüştürüyoruz.
            _context.Features.Add(values);//Veritabanına ekliyoruz.
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteFeature(int id)
        {
            var values = _context.Features.Find(id);
            _context.Features.Remove(values);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpGet("GetFeature")]
        public IActionResult GetFeature(int id)
        {
            var values = _context.Features.Find(id);
            return Ok(_mapper.Map<GetByIdFeatureDto>(values));//Feature'ı GetByIdFeatureDto'ya dönüştürüp döndürüyoruz.
        }
        [HttpPut]
        public IActionResult UpdateFeature(UpdateFeaturedTO updateFeaturedTO)
        {
            var values = _mapper.Map<Feature>(updateFeaturedTO);//UpdateFeaturedTO'yu Feature'a dönüştürüyoruz.
            _context.Features.Update(values);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }
    }
}
