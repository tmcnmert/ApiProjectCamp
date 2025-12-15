using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly ApiContext _context;//bağlantı için context tanımlandı
        public ChefsController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult ChefList()
        {
            var chefs = _context.Chefs.ToList();
            return Ok(chefs);
        }

        [HttpPost]
        public IActionResult CreateChef(Chef chef)
        {
            _context.Chefs.Add(chef);
            _context.SaveChanges();
            return Ok("Şef ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteChef(int id)
        {
            var chef = _context.Chefs.Find(id);
            _context.Chefs.Remove(chef);
            _context.SaveChanges();
            return Ok("Şef silme işlemi başarılı");
        }
        [HttpGet("GetChef")]
        public IActionResult GetChef(int id)
        {
            var chef = _context.Chefs.Find(id);
            return Ok(chef);
        }
        [HttpPut]
        public IActionResult UpdateChef(Chef chef)
        {
           _context.Chefs.Update(chef);
            _context.SaveChanges();
            return Ok("Şef güncelleme işlemi başarılı");
        }
    }
}
