using EmployeeCRUDAPI.DatabbaseContext;
using EmployeeCRUDAPI.DTOs;
using EmployeeCRUDAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.Employees.AsNoTracking().Where(x => x.IsActive).OrderByDescending(x => x.Id).ToListAsync();
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id && x.IsActive);

            if (employee == null)
            {
                return NotFound(new {Message="Employee not found"});
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDTO request)
        {
            var employee = new Employee
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                Department = request.Department.Trim(),
                Salary = request.Salary,
                IsActive = true,
                CreatedOn=DateTime.UtcNow
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id=employee.Id}, employee);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDTO request)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x=>x.Id==id && x.IsActive);

            if(employee == null)
            {
                return NotFound(new {Message="Employee not found"});
            }

            employee.FullName = request.FullName.Trim();
            employee.Email = request.Email.Trim();
            employee.Department = request.Department.Trim();
            employee.Salary=    request.Salary;

            await _context.SaveChangesAsync();

            return Ok(new {Message="Employee Updated Successfully"});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if(employee == null)
            {
                return NotFound(new {Message = "Employee not found."});
            }

            employee.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new {Message = "Employee deleted successfully"});
        }
    }
}
