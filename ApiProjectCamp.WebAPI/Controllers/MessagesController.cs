using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Dtos.MessageDtos;
using ApiProjectCamp.WebAPI.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;
        public MessagesController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult MessageList()
        {
            var values = _context.Messages.ToList();
            return Ok(_mapper.Map<List<ResultMessageDto>>(values));
        }
        [HttpPost]
        public IActionResult CreateMessage(CreateMessageDto createMessageDto)
        {
            var values = _mapper.Map<Message>(createMessageDto);
            _context.Messages.Add(values);
            _context.SaveChanges();
            return Ok("Mesaj ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteMessage(int id)
        {
            var values = _context.Messages.Find(id);
            _context.Messages.Remove(values);
            _context.SaveChanges();
            return Ok("Mesaj silme işlemi başarılı");
        }
        [HttpGet("GetMessage")]
        public IActionResult GetMessage(int id)
        {
            var values = _context.Messages.Find(id);
            return Ok(_mapper.Map<GetByIdMessageDto>(values));
        }
        [HttpPut]
        public IActionResult UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var values = _mapper.Map<Message>(updateMessageDto);
            _context.Messages.Update(values);
            _context.SaveChanges();
            return Ok("Mesaj güncelleme işlemi başarılı");
        }

        [HttpGet("MessageListByIsReadFalse")]
        public IActionResult MessageListByIsReadFalse()
        {
            var value=_context.Messages.Where(x=>x.IsRead==false).ToList();
            return Ok(value);
        }
    }
}
