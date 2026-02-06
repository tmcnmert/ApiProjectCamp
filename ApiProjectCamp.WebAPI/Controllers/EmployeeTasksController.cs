using ApiProjectCamp.WebAPI.Context;
using ApiProjectCamp.WebAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTasksController : ControllerBase
    {
        private readonly ApiContext _context;

        public EmployeeTasksController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult EmployeeTaskList()
        {
            var employeeTask = _context.EmployeeTasks.ToList();
            return Ok(employeeTask);
        }

        [HttpPost]
        public IActionResult CreateEmployeeTask(EmployeeTask employeeTask)
        {
            _context.EmployeeTasks.Add(employeeTask);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }
        [HttpDelete]
        public IActionResult DeleteEmployeeTask(int id)
        {
            var employeeTask = _context.EmployeeTasks.Find(id);
            _context.EmployeeTasks.Remove(employeeTask);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı");
        }
        [HttpGet("GetEmployeeTask")]
        public IActionResult GetEmployeeTask(int id)
        {
            var employeeTask = _context.EmployeeTasks.Find(id);
            return Ok(employeeTask);
        }
        [HttpPut]
        public IActionResult UpdateEmployeeTask(EmployeeTask employeeTask)
        {
            _context.EmployeeTasks.Update(employeeTask);
            _context.SaveChanges();
            return Ok("Güncelleme işlemi başarılı");
        }
    }
}
