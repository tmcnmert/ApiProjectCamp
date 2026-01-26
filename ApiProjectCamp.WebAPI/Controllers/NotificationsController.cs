using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Dtos.NotificationDtos;
using ApiProjectCamp.WebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;

        public NotificationsController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult NotificationList()
        {
            var values = _context.Notifications.ToList();
            return Ok(_mapper.Map<List<ResultNotificationDto>>(values));//Notification listesini ResultNotificationDto'ya dönüştürüp döndürüyoruz.
        }
        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDto createNotificationDto)
        {
            var values = _mapper.Map<Notification>(createNotificationDto);//CreateNotificationDto'yı Notification'a dönüştürüyoruz.
            _context.Notifications.Add(values);//Veritabanına ekliyoruz.
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteNotification(int id)
        {
            var values = _context.Notifications.Find(id);
            _context.Notifications.Remove(values);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpGet("GetNotification")]
        public IActionResult GetNotification(int id)
        {
            var values = _context.Notifications.Find(id);
            return Ok(_mapper.Map<GetNotificationByIdDto>(values));//Notification'ı GetByIdNotificationDto'ya dönüştürüp döndürüyoruz.
        }
        [HttpPut]
        public IActionResult UpdateNotification(UpdateNotificationDto updateNotificationDto)
        {
            var values = _mapper.Map<Notification>(updateNotificationDto);//UpdateNotificationdTO'yu Notification'a dönüştürüyoruz.
            _context.Notifications.Update(values);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }
    }
}
